using ASP_.NET_nauka.Data;
using Microsoft.AspNetCore.Mvc;
using ASP_.NET_nauka.Models;
using System.Security.Claims;
namespace ASP_.NET_nauka.Controllers;

[Route("Trade/{currencyId}")]
public class TradeController : Controller
{
	private MyDbContext _db;

	public TradeController(MyDbContext db)
	{
		_db = db;
	}

    [HttpGet("Buy")]
    public IActionResult BuyPartial(string currencyId)
    {
        WalletCurrencyValue curValue = GetCurrencyValue(currencyId);
        return PartialView("_Buy", curValue);
    }

    [HttpGet("Sell")]
    public IActionResult SellPartial(string currencyId)
    {
		WalletCurrencyValue curValue = GetCurrencyValue(currencyId);
        return PartialView("_Sell", curValue);
    }

    public IActionResult Index(string currencyId)
	{
		Currency? currency = _db.Currencies.Where(x => x.Id == currencyId).FirstOrDefault();
		if (currency == null)
		{
			return RedirectToAction("Index", "Home");
		}
        
        TradeChartPackage package = CreatePackage(currency);
		

		return View(package);
	}

    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Buy()
    {
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sell()
    {

        return RedirectToAction("Index");
    }*/

	private TradeChartPackage CreatePackage(Currency currency)
	{
        TradeChartPackage package = new TradeChartPackage();
        package.Currency = currency;

        package.CurrencyHistory = _db.CurrenciesHistory
            .Where(x => x.CurrencyId == currency.Id)
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        WalletCurrencyValue? walletCurrencyValue = new();

        if (userId == null)
        {
            package.WalletCurrencyValue = new WalletCurrencyValue(currency.Id);
            return package;
        }

        var user = _db.Users.Where(x => x.Id == int.Parse(userId)).FirstOrDefault();
        if (user != null)
        {
            package.User = user;
        } 

        walletCurrencyValue = _db.WalletCurrencyValues
            .FirstOrDefault(x => x.UserId == int.Parse(userId)
            && x.CurrencyId == currency.Id);

        if (walletCurrencyValue == null)
        {
            walletCurrencyValue = new WalletCurrencyValue();
            walletCurrencyValue.CurrencyId = currency.Id;
            walletCurrencyValue.UserId = int.Parse(userId);
            walletCurrencyValue.Value = 0;
        }
        package.WalletCurrencyValue = walletCurrencyValue;
        return package;
    }

    private WalletCurrencyValue GetCurrencyValue(string currencyId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return new WalletCurrencyValue(currencyId); 
        }
        var walletCurrencyValue = _db.WalletCurrencyValues
            .Where(x => x.UserId == int.Parse(userId) && x.CurrencyId == currencyId)
            .FirstOrDefault();
        if (walletCurrencyValue == null)
        {
            return new WalletCurrencyValue(currencyId);
        }
        return walletCurrencyValue;
    }
}
