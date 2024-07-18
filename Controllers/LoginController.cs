using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Humanizer;
using Microsoft.EntityFrameworkCore;
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
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginTemplate loginTemplate)
        {
            string HashedPassword = Models.User.CreateSHA256Hash(loginTemplate.Password);
            var User = _db.Users
                .Where(x => x.Email == loginTemplate.Email && x.Password == HashedPassword)
                .FirstOrDefault();
            if (User != null)
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                    new Claim(ClaimTypes.Name, User.FirstName)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, 
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = loginTemplate.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "User not found";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignUpSubmit(User user)
        {
            var _user = _db.Users.FirstOrDefault(x => x.Email == user.Email);
            if (_user != null)
            {
                TempData["AccountError"] = "Email already exists";
                return RedirectToAction("SignUp");
            }

            user.Password = Models.User.CreateSHA256Hash(user.Password);
            user.RoleId = (from role in _db.Roles
                        where role.Name == "User"
                        select role.Id).FirstOrDefault();
            user.USDT_balance = 1000;

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");  
        }

       
    }
}
