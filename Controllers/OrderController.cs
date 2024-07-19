using System.Security.Claims;
using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Hubs;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ASP_.NET_nauka.Controllers;


public class OrderController : Controller
{
    private readonly MyDbContext _db;

    private readonly IHubContext<OrderHub> _hubContext;

    public OrderController(MyDbContext db, IHubContext<OrderHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Buy(OrderPackage newOrder, string currencyId)
    {

        if (string.IsNullOrEmpty(currencyId) || !CheckValues(newOrder))
        {
            return RedirectToAction("Index", "Home");
        }

        int UserId;
        if (!TryGetUserId(out UserId))
        {
            TempData["OrderSuccessMessage"] = "You need to be looged";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }


        decimal value;
        if (!TryGetCurrencyValue(currencyId, out value))
        {
            TempData["OrderSuccessMessage"] = "Currency not available";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }

        decimal QtyUSDT;
        if (!decimal.TryParse(newOrder.QtyUSDT.Replace('.', ','), out QtyUSDT))
        {
            TempData["OrderSuccessMessage"] = "Wrong data format";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }

        User? user = _db.Users.Where(x => x.Id == UserId).FirstOrDefault();
        if (user == null || user.USDT_balance - QtyUSDT <= 0)
        {
            TempData["OrderSuccessMessage"] = "Not enough money";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }

        user.USDT_balance -= QtyUSDT;

        decimal Qty = QtyUSDT / value;

        ActiveOrder activeOrder = new ActiveOrder()
        {
            Qty = Qty,
            QtyUSDT = QtyUSDT,
            DateOfIssue = DateTime.Now,
            UserId = UserId,
            CurrencyId = currencyId,
            IsBuyer = true
        };
        _db.ActiveOrders.Add(activeOrder);
        _db.SaveChanges();

        TempData["OrderSuccessMessage"] = "Order created successful";


        IEnumerable<ActiveOrder> activeOrders = _db.ActiveOrders.ToList();
        _hubContext.Clients.All.SendAsync(currencyId, activeOrders);


        return RedirectToAction("Index", "Trade", new { currencyId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sell(OrderPackage newOrder, string currencyId)
    {
        Console.WriteLine(newOrder.QtyUSDT);
        Console.WriteLine(newOrder.Qty);


        if (!string.IsNullOrEmpty(currencyId))
        {
            return RedirectToAction("Index", "Trade", new { currencyId });
        }
        return RedirectToAction("Index", "Home");
    }

    private bool TryGetCurrencyValue(string currencyId, out decimal value)
    {
        decimal? _value = _db.Currencies
            .Where(x => x.Id == currencyId)
            .Select(x => x.Value).FirstOrDefault();
        if (_value == null || _value == 0)
        {
            value = -1;
            return false;
        }
        value = _value.Value;
        return true;
    }

    private bool TryGetUserId(out int UserId)
    {
        var StringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (StringUserId == null)
        {
            UserId = -1;
            return false;
        }
        UserId = int.Parse(StringUserId);
        return true;
    }

    private bool CheckValues(OrderPackage newOrder)
    {
        if (newOrder.Qty == string.Empty
            || newOrder.QtyUSDT == string.Empty
            || newOrder.CurrencyId == string.Empty)
        {
            return false;
        }
        return true;
    }
}
