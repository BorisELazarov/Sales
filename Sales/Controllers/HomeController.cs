using Microsoft.AspNetCore.Mvc;
using Sales.ActionFilters;
using Sales.Entities;
using Sales.ExtensionMethods;
using Sales.Repositories;
using Sales.ViewModels.Home;
using Sales.ViewModels.Products;
using System.Diagnostics;

namespace Sales.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            UsersRepository repo = new UsersRepository();
            User loggedUser = repo.GetFirstOrDefault(u => u.Username == model.Username &&
            u.Password == model.Password);

            if (loggedUser == null)
            {
                this.ModelState.AddModelError("authError","Incorrect username or password");
                return View(model);
            }

            HttpContext.Session.SetObject<User>("loggedUser",loggedUser);
            HttpContext.Session.SetObject<List<AddToCartVM>>("cart", new List<AddToCartVM>());

            return RedirectToAction("Index");
        }


        [AuthenticationFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}