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
using static System.Runtime.InteropServices.JavaScript.JSType;

public class DataUpdater : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IHubContext<CurrenciesHub> _hubContext;
	private readonly HttpClient _httpClient;
	public DataUpdater(IServiceProvider serviceProvider, IHubContext<CurrenciesHub> hubContext, HttpClient httpClient)
	{
		_serviceProvider = serviceProvider;
		_hubContext = hubContext;
		_httpClient = httpClient;
	}
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
			if (dbContext == null)
			{
				throw new Exception("Service provider error");
			}

			UpdateDatabase(dbContext);
			while (!stoppingToken.IsCancellationRequested)
			{
				await UpdateCurrencies(dbContext, _httpClient);
				CalculateChange(dbContext);
				IEnumerable<Currency> currencies = dbContext.Currencies.ToList();
				await _hubContext.Clients.All.SendAsync("ReceiveCurrencies", currencies);
				Console.WriteLine("----------DBUpdate----------");
				await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
			}
		}

	}
	public static async Task UpdateCurrencies(MyDbContext _db, HttpClient client)
	{
		List<string> CurrenciesIDs = _db.Currencies.Select(x => x.Id).ToList();
		
		string url = "https://api.coingecko.com/api/v3/simple/price?ids=" + string.Join(",", CurrenciesIDs) + "&vs_currencies=usd";
		var data = client.GetAsync(url).Result;
		if (data.IsSuccessStatusCode)
		{
			string json = await data.Content.ReadAsStringAsync();
			dynamic? jsonObject = JsonConvert.DeserializeObject(json);
			if (jsonObject == null)
			{
				Console.WriteLine("Data is null");
				return;
			}
			Console.WriteLine(jsonObject);
			foreach (Currency currency in _db.Currencies)
			{
				decimal NewValue = decimal.Parse(jsonObject[currency.Id]["usd"].ToString());
				currency.Value = NewValue;
				if (currency.Low > NewValue || currency.Low == 0) currency.Low = NewValue;
				if (currency.High < NewValue) currency.High = NewValue;
				currency.Date = DateTime.Now;
			}

			//Console.WriteLine(decimal.Parse(jsonObject[Id]["usd"].ToString()));

			_db.SaveChanges();
		}
		else
		{
			Console.WriteLine("Data error");
		}
		
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
	
	public static async Task<bool> CreateCurrencyHistory(string CurrencyID, int Days, MyDbContext _db, HttpClient client)
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

				var NewestHistoryRecordDate = (_db.CurrenciesHistory
					.Where(x => x.CurrencyId == CurrencyID)
					.Max(x => x.Date)).Date;
				if (NewestHistoryRecordDate == DateTime.Parse(date))
				{
					Console.WriteLine("API not available yet");
					return false;
				}


				decimal CurLow = decimal.MaxValue;
				decimal CurHigh = decimal.MinValue;
					
					
				decimal Open = decimal.Parse(Prices[Day*24].Item2);
				decimal Close = decimal.Parse(Prices[Day * 24 + 23].Item2);
				for (int Hour = 0; Hour < 24; Hour++)
				{
					int index = Day * 24 + Hour;
					decimal _price = decimal.Parse(Prices[index].Item2);
						
					if (_price < CurLow) CurLow = _price;
					if (_price > CurHigh) CurHigh = _price;
				}
					
				CurrencyHistory currencyHistory = new CurrencyHistory();
				currencyHistory.CurrencyId = CurrencyID;
				currencyHistory.Low = CurLow;	
				currencyHistory.High = CurHigh;
				currencyHistory.Open = Open;
				currencyHistory.Close = Close;
				currencyHistory.Date = DateTime.Parse(date);
				_db.CurrenciesHistory.Add(currencyHistory);
			}

			_db.SaveChanges();
				
		}
		else
		{
			throw new Exception("Error: something wrong with data");
		}
		
		return true;
	}

	public void UpdateDatabase(MyDbContext _db)
	{
		var CurrencyIDs = _db.Currencies.Select(x => x.Id).ToList();
	
		DateTime CurrentDate = DateTime.Now.Date;
		foreach (var currencyID in CurrencyIDs)
		{
			var NewestHistoryRecordDate = _db.CurrenciesHistory
				.Where(x => x.CurrencyId == currencyID)
				.Max(x => x.Date);
			int DifferenceInDays = (CurrentDate - NewestHistoryRecordDate).Days;
			if (DifferenceInDays > 1)
			{

				Task<bool> CreateTask = CreateCurrencyHistory(currencyID, DifferenceInDays - 1, _db, _httpClient);
				CreateTask.Wait();
				if (!CreateTask.Result) return;
				Console.WriteLine($"Created {DifferenceInDays - 1} days history for {currencyID}");
				Task.Delay(15000).Wait();
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
										  ch.Date,
										  ch.Close
									  }).ToList();

		foreach (Currency currency in _db.Currencies)
		{
			var LastKnown = result.FirstOrDefault(x => x.CurrencyId == currency.Id);
			if (LastKnown != null )
			{
				decimal change = ((currency.Value - LastKnown.Close) / LastKnown.Close) * 100;
				currency.Change = Math.Round(change, 2);
			}
		}
		_db.SaveChanges();
	}

}

