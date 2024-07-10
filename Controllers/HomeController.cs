using System.Diagnostics;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ASP_.NET_nauka.Data;
namespace ASP_.NET_nauka.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private MyDbContext _db;

	public HomeController(ILogger<HomeController> logger, MyDbContext db)
	{
		_logger = logger;
		_db = db;
	}

	public IActionResult Index()
	{
		IEnumerable<Currency> currencies = _db.Currencies.ToList();
		return View(currencies);
	}

	public IActionResult Privacy()
	{
		return View();
	}


	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
