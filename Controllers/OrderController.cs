using System.Security.Claims;
using System.Threading.Channels;
using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Hubs;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

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

        if (string.IsNullOrEmpty(currencyId))
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
        if (QtyUSDT == 0)
        {
			TempData["OrderSuccessMessage"] = "QtyUSDT must be greater than 0";
			return RedirectToAction("Index", "Trade", new { currencyId });
		}


        User? user = _db.Users.Where(x => x.Id == UserId).FirstOrDefault();
        if (user == null || user.USDT_balance - QtyUSDT <= 0)
        {
            TempData["OrderSuccessMessage"] = "Not enough money";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }

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

        ManageActiveBuyOrders();

        IEnumerable<CompletedOrder> completedOrders = _db.CompletedOrders
            .OrderByDescending(x => x.EndDate)
            .Take(10)
            .ToList();
        _hubContext.Clients.All.SendAsync($"{currencyId}/Buyers", completedOrders);

        TempData["OrderSuccessMessage"] = "Order successfully created";
        return RedirectToAction("Index", "Trade", new { currencyId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sell(OrderPackage newOrder, string currencyId)
    {
        if (string.IsNullOrEmpty(currencyId))
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

        decimal Qty;
        if (!decimal.TryParse(newOrder.Qty.Replace('.', ','), out Qty))
        {
            TempData["OrderSuccessMessage"] = "Wrong data format";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }
        if (Qty == 0)
        {
			TempData["OrderSuccessMessage"] = "Qty must be greater than 0";
			return RedirectToAction("Index", "Trade", new { currencyId });
		}

        User? user = _db.Users.Where(x => x.Id == UserId).FirstOrDefault();

        if (user == null)
        {
            TempData["OrderSuccessMessage"] = "User error";
            return RedirectToAction("Index", "Trade", new { currencyId });
        }

        decimal QtyUSDT = Qty * value;

        ActiveOrder activeOrder = new ActiveOrder()
        {
            Qty = Qty,
            QtyUSDT = QtyUSDT,
            DateOfIssue = DateTime.Now,
            UserId = UserId,
            CurrencyId = currencyId,
            IsBuyer = false
        };
        _db.ActiveOrders.Add(activeOrder);
        _db.SaveChanges();

        ManageActiveSellOrders();

        IEnumerable<CompletedOrder> completedOrders = _db.CompletedOrders
            .OrderByDescending(x => x.EndDate)
            .Take(10)
            .ToList();
        _hubContext.Clients.All.SendAsync($"{currencyId}/Sellers", completedOrders);

        TempData["OrderSuccessMessage"] = "Order successfully created";
        return RedirectToAction("Index", "Trade", new { currencyId });
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


    private void ManageActiveBuyOrders()
    {
        IEnumerable<ActiveOrder> activeOrders = _db.ActiveOrders
            .Where(x => x.IsBuyer == true)
            .ToList();
        foreach (var activeOrder in activeOrders)
        {
            _db.ActiveOrders.Remove(activeOrder);
            User? user = _db.Users.Where(x => x.Id == activeOrder.UserId).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("UserId order error");
                continue;
            }
            if (user.USDT_balance < activeOrder.QtyUSDT)
            {
                Console.WriteLine("Not enough USDT");
                continue;
            }

            user.USDT_balance -= activeOrder.QtyUSDT;
        
            WalletCurrencyValue? WCV = _db.WalletCurrencyValues
                .Where(x => x.UserId == activeOrder.UserId 
                && x.CurrencyId == activeOrder.CurrencyId)
                .FirstOrDefault();
            if (WCV == null)
            {
                WCV = new();
                WCV.CurrencyId = activeOrder.CurrencyId;
                WCV.UserId = activeOrder.UserId;
                WCV.Value = activeOrder.Qty;
                _db.WalletCurrencyValues.Add(WCV);  
            } else
            {
                WCV.Value += activeOrder.Qty;
            }

            CompletedOrder completedOrder = new CompletedOrder(activeOrder, DateTime.Now);
            _db.CompletedOrders.Add(completedOrder);

            _db.SaveChanges();
        }
    }

    private void ManageActiveSellOrders()
    {
        IEnumerable<ActiveOrder> activeOrders = _db.ActiveOrders
            .Where(x => x.IsBuyer == false)
            .ToList();
        foreach (var activeOrder in activeOrders)
        {
            _db.ActiveOrders.Remove(activeOrder);
            User? user = _db.Users.Where(x => x.Id == activeOrder.UserId).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("UserId order error");
                continue;
            }

            WalletCurrencyValue? WCV = _db.WalletCurrencyValues
                .Where(x => x.UserId == activeOrder.UserId
                && x.CurrencyId == activeOrder.CurrencyId)
                .FirstOrDefault();
            if (WCV == null)
            {
                Console.WriteLine("Currency does not exists in the user wallet");
                continue;
            }

            if (WCV.Value < activeOrder.Qty)
            {
                Console.WriteLine("Not enough Qty in user wallet");
                continue;
            }
            WCV.Value -= activeOrder.Qty;
            user.USDT_balance += activeOrder.QtyUSDT;

            CompletedOrder completedOrder = new CompletedOrder(activeOrder, DateTime.Now);
            _db.CompletedOrders.Add(completedOrder);

            _db.SaveChanges();
        }
    }
}
