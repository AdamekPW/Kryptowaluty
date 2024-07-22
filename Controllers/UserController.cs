using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ASP_.NET_nauka.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private MyDbContext _db;
        public UserController(MyDbContext db) 
        { 
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
		
		public IActionResult WalletPartial()
        {
            var StringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (StringUserId == null)
            {
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
            int UserId = int.Parse(StringUserId);
            var Wallet = _db.Wallets.FirstOrDefault(x => x.UserId == UserId);
            if (Wallet == null)
            {
                Wallet = new Wallet();  
            }
            WalletPackage WP = new WalletPackage(Wallet, _db);
            

            return PartialView("_Wallet", WP);
        }
        public IActionResult HistoryPartial()
        {
            var StringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (StringUserId == null)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            int UserId = int.Parse(StringUserId);
            List<HistoryRecord> history = new();

            IEnumerable<CompletedOrder> userOrders = _db.CompletedOrders
                .Where(x => x.UserId == UserId).ToList();
            foreach (var order in userOrders)
            {
                Currency? currency = _db.Currencies.FirstOrDefault(x => x.Id == order.CurrencyId);
                if (currency == null) continue;
                HistoryRecord hr = new HistoryRecord();
                hr.Currency = currency;
                hr.CompletedOrder = order;
                history.Add(hr);
            }


            return PartialView("_History", history);
        }

        [HttpGet]
        public IActionResult SettingsPartial()
        {
			var StringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (StringUserId == null)
			{
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
			int UserId = int.Parse(StringUserId);
            User? user = _db.Users.Where(x => x.Id == UserId).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
			return PartialView("_Settings", user);
        }
        [HttpPost]
        public IActionResult ChangePassword(User user)
        {
			var StringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (StringUserId == null)
			{
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
			int UserId = int.Parse(StringUserId);
			User? DbUser = _db.Users.Where(x => x.Id == UserId).FirstOrDefault();
			if (DbUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			string HashedPassword = Models.User.CreateSHA256Hash(user.Password);
            if (DbUser.Password == HashedPassword)
            {
				TempData["PasswordChange"] = "Password cannot be the same";
				return RedirectToAction("Index");
			}

            DbUser.Password = HashedPassword;

            _db.SaveChanges();

            TempData["PasswordChange"] = "Password changed successfuly";
            return RedirectToAction("Index");
        }


    }
}
