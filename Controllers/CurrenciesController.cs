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
			DateTime maxDate = _db.CurrenciesHistory.Max(ch => ch.Date);

			var result = _db.CurrenciesHistory.Where(ch => ch.Date == maxDate)
										  .Select(ch => new
										  {
											  ch.CurrencyId,
											  ch.AvgValue,
											  ch.Date
										  }).ToList();

            foreach (Currency currency in CurrenciesList)
            {
              
                var LastKnown = result.FirstOrDefault(x => x.CurrencyId == currency.Id);
                if (LastKnown != null && LastKnown.AvgValue != 0)
                {
                    decimal change = ((currency.Value - LastKnown.AvgValue) / LastKnown.AvgValue)*100;
                    currency.Change = change;
                } 
            }

			return View(CurrenciesList);
        }
    }
}
