using ASP_.NET_nauka.Data;
using Microsoft.AspNetCore.Mvc;
using ASP_.NET_nauka.Models;
namespace ASP_.NET_nauka.Controllers;

public class TradeController : Controller
{
	private MyDbContext _db;

	public TradeController(MyDbContext db)
	{
		_db = db;
	}

	[Route("Trade/{currencyId}")]
	public IActionResult Index(string currencyId)
	{
		Currency? currency = _db.Currencies.Where(x => x.Id == currencyId).FirstOrDefault();
		if (currency == null)
		{
			return RedirectToAction("Index", "Home");
		}
        
		TradeChartPackage package = new TradeChartPackage();
		package.Currency = currency;

		package.CurrencyHistory = _db.CurrenciesHistory
			.Where(x => x.CurrencyId == currencyId)
			.OrderBy(x => x.Date)
			.ToList();

		var item = package.CurrencyHistory[0];
        item.Open = FrontendFunctions.Make5Digits(item.Open);
        item.Low = FrontendFunctions.Make5Digits(item.Close);
        item.High = FrontendFunctions.Make5Digits(item.High);
        item.Close = FrontendFunctions.Make5Digits(item.Close);
        for (int i = 1; i < package.CurrencyHistory.Count; i++)
		{
			item = package.CurrencyHistory[i];
			item.Open = FrontendFunctions.Make5Digits(package.CurrencyHistory[i - 1].Close);
            item.Low = FrontendFunctions.Make5Digits(item.Low);
            item.High = FrontendFunctions.Make5Digits(item.High);
            item.Close = FrontendFunctions.Make5Digits(item.Close);
        }


		return View(package);
	}
}
