using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_nauka.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
