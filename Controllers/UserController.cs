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
        public IActionResult SettingsPartial()
        {
            return PartialView("_Settings");
        }
        public IActionResult HistoryPartial()
        {
            return PartialView("_History");
        }

    }
}
