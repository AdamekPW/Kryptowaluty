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

	[Route("trade/{currencyId}")]
	public IActionResult Index(Currency currencyId)
	{
		return View();
	}
}
