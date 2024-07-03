using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_nauka.Controllers
{
    public class LoginController : Controller
    {
        private MyDbContext _db;
        public LoginController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine("Hello");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User user)
        {
            Console.WriteLine(user.Email);
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
