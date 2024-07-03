using System.ComponentModel;
using System.Diagnostics.Contracts;
using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Hubs;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

public class DataUpdater : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IHubContext<CurrenciesHub> _hubContext;
	public DataUpdater(IServiceProvider serviceProvider, IHubContext<CurrenciesHub> hubContext)
	{
		_serviceProvider = serviceProvider;
		_hubContext = hubContext;
	}
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using (var scope = _serviceProvider.CreateScope()) { 
			var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
			if (dbContext == null)
			{
				throw new Exception("Service provider error");
			}

            UpdateDatabase(dbContext);
			while (!stoppingToken.IsCancellationRequested)
			{
				await UpdateCurrencies(dbContext);
				CalculateChange(dbContext);
				IEnumerable<Currency> currencies = dbContext.Currencies.ToList();
				await _hubContext.Clients.All.SendAsync("ReceiveCurrencies", currencies);
                Console.WriteLine("----------DBUpdate----------");
				await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
			}
		}
		
	}

	private static async Task<List<Currency>> GetCurrenciesByIDs(List<string> CurrenciesIDs)
	{
		List<Currency> Data = new();

		using (var client = new HttpClient())
		{

			string url = "https://api.coingecko.com/api/v3/simple/price?ids=" + string.Join(",", CurrenciesIDs) + "&vs_currencies=usd";
			var data = client.GetAsync(url).Result;
			if (data.IsSuccessStatusCode)
			{
				string json = await data.Content.ReadAsStringAsync();
				dynamic? jsonObject = JsonConvert.DeserializeObject(json);
				if (jsonObject == null)
				{
					Console.WriteLine("Data is null");
					return Data;
				}

				foreach (var Id in CurrenciesIDs)
				{
					Currency cryptoCurrency = new();
					cryptoCurrency.Id = Id;
					cryptoCurrency.Value = decimal.Parse(jsonObject[Id]["usd"].ToString());
					(string, string) NameAndCode = GetCurrencyNameAndCodeById(Id);
					cryptoCurrency.Name = NameAndCode.Item1;
					cryptoCurrency.Code = NameAndCode.Item2;
					cryptoCurrency.Measurement = 0;
					cryptoCurrency.Sum = 0;
					cryptoCurrency.High = 0;
					cryptoCurrency.Low = 0;
					cryptoCurrency.Change = 0;
					Data.Add(cryptoCurrency);
				}
				

			} else
			{
				Console.WriteLine("Data error");
			}
		}
		return Data;
	}

	private static (string, string) GetCurrencyNameAndCodeById(string Currency)
	{
		if (!File.Exists("AllCoins.xlsx"))
		{
			throw new Exception("File named AllCoins.xlsx does not exits");
		}

		FileInfo file = new FileInfo("AllCoins.xlsx");
		ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
		using (ExcelPackage package = new ExcelPackage(file))
		{
			ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
			int lastRow = worksheet.Dimension.End.Row;
			for (int i = 1; i <= lastRow; i++)
			{
				object? ID = worksheet.Cells[i, 1].Value;
				if (ID != null && ID.ToString() == Currency)
				{
					return (worksheet.Cells[i, 3].Value.ToString(), worksheet.Cells[i, 2].Value.ToString());
				}

			}

		}

		return ("", "");
	}
	
	public async Task UpdateCurrencies(MyDbContext dbContext)
	{
		List<string> currencies = dbContext.Currencies.Select(x => x.Id).ToList();
		var T = GetCurrenciesByIDs(currencies);
		T.Wait();
		foreach (Currency currency in T.Result)
		{

			var Entity = dbContext.Currencies.FirstOrDefault(x => x.Id == currency.Id);

			if (Entity != null)
			{
				if (Entity.Date.Date == DateTime.Now.Date)
				{
					Entity.Value = currency.Value;
					if (Entity.Low > currency.Value || Entity.Low == 0) Entity.Low = currency.Value;
					if (Entity.High < currency.Value) Entity.High = currency.Value;
					Entity.Sum += currency.Value;
					Entity.Measurement++;
					Entity.Date = DateTime.Now;

				} else
				{
					//calculate avg and write to history 
					CurrencyHistory currencyHistory = new();
					currencyHistory.AvgValue = Entity.Sum / Entity.Measurement;
					currencyHistory.Low = Entity.Low;
					currencyHistory.High = Entity.High;
					currencyHistory.Date = DateTime.Now;
					currencyHistory.CurrencyId = currency.Id;
					dbContext.CurrenciesHistory.Add(currencyHistory);

					Entity.Value = currency.Value;
					Entity.Low = currency.Value;
					Entity.High = currency.Value;
					Entity.Sum = currency.Value;
					Entity.Measurement = 1;
					Entity.Date = DateTime.Now;
					Console.WriteLine("New history record created");
				}

			} else
			{
				Console.WriteLine($"Entity {currency.Id} not found");

			}
		}
		await dbContext.SaveChangesAsync();
	}

	public static async Task CreateCurrencyHistory(string CurrencyID, MyDbContext dbContext, int Days)
	{
		List<CurrencyHistory> history = new List<CurrencyHistory>();

		using (var client = new HttpClient())
		{
			string url = $"https://api.coingecko.com/api/v3/coins/{CurrencyID}/market_chart?vs_currency=usd&days={Days}";
			var data = client.GetAsync(url).Result;
			if (data.IsSuccessStatusCode)
			{
				string json = await data.Content.ReadAsStringAsync();
				dynamic? jsonObject = JsonConvert.DeserializeObject(json);
				JArray prices = jsonObject["prices"];
				List<(string, string)> Prices = prices.Select(x => ( x[0].ToString(), x[1].ToString())).ToList();

				for (int Day = 0; Day < Days; Day++)
				{
					DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(Prices[Day*24].Item1));
					string date = dateTimeOffset.ToString("yyyy-MM-dd");

					decimal Sum = 0;
					decimal CurLow = decimal.MaxValue;
					decimal CurHigh = decimal.MinValue;
					for (int Hour = 0; Hour < 24; Hour++)
					{
						int index = Day * 24 + Hour;
						decimal _price = decimal.Parse(Prices[index].Item2);
						Sum += _price;
						if (_price < CurLow) CurLow = _price;
						if (_price > CurHigh) CurHigh = _price;
					}
					
					CurrencyHistory currencyHistory = new CurrencyHistory();
					currencyHistory.CurrencyId = CurrencyID;
					currencyHistory.AvgValue = Sum/24;
					currencyHistory.Low = CurLow;	
					currencyHistory.High = CurHigh;
					currencyHistory.Date = DateTime.Parse(date);
					dbContext.CurrenciesHistory.Add(currencyHistory);
				}

				dbContext.SaveChanges();
				
			}
			else
			{
				throw new Exception("Error: something wrong with data");
			}
		}
	}

	public static void UpdateDatabase(MyDbContext _db)
	{
		var CurrencyIDs = _db.Currencies.Select(x => x.Id).ToList();
		//Sprawdź jaka dzisiaj jest data i jaka jest data ostatniego wpisu w historii 
		DateTime CurrentDate = DateTime.Now.Date;
		foreach (var currencyID in CurrencyIDs)
		{
			var NewestHistoryRecordDate = _db.CurrenciesHistory
				.Where(x => x.CurrencyId == currencyID)
				.Max(x => x.Date);
			int DifferenceInDays = (CurrentDate - NewestHistoryRecordDate).Days;
			if (DifferenceInDays > 1)
			{
				CreateCurrencyHistory(currencyID, _db, DifferenceInDays-1).Wait();
				Console.WriteLine($"Created {DifferenceInDays-1} days history for {currencyID}");
				Task.Delay(15000).Wait() ;
			} else
			{
				Console.WriteLine($"Data for {currencyID} in CurrencyHistory is up to date");
			}
		}
		
    }

	private void CalculateChange(MyDbContext _db)
	{
		DateTime maxDate = _db.CurrenciesHistory.Max(ch => ch.Date);

		var result = _db.CurrenciesHistory.Where(ch => ch.Date == maxDate)
									  .Select(ch => new
									  {
										  ch.CurrencyId,
										  ch.AvgValue,
										  ch.Date
									  }).ToList();

		foreach (Currency currency in _db.Currencies)
		{
			var LastKnown = result.FirstOrDefault(x => x.CurrencyId == currency.Id);
			if (LastKnown != null && LastKnown.AvgValue != 0)
			{
				decimal change = ((currency.Value - LastKnown.AvgValue) / LastKnown.AvgValue) * 100;
				currency.Change = Math.Round(change,2);
			}
		}
		_db.SaveChanges();
	}
}

