using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_nauka.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
