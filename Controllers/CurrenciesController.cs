using System.ComponentModel.DataAnnotations;
using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_nauka.Controllers
{
    
    public class CurrenciesController : Controller
    {
        private readonly MyDbContext _db;
        public CurrenciesController(MyDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Currency> CurrenciesList = _db.Currencies;
			return View(CurrenciesList);
        }
    }
}
