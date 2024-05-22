using Microsoft.AspNetCore.Mvc;
using Sales.Repositories;
using Sales.ViewModels.Users;
using Sales.ViewModels.Shared;
using Sales.Entities;
using System.Linq.Expressions;
using Sales.ExtensionMethods;

namespace Sales.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Index(IndexVM model)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            UsersRepository repo = new UsersRepository();

            model.Pager ??=new PagerVM();

            if (model.Pager.Page <= 0)
               model.Pager.Page= 1;

            if (model.Pager.ItemsPerPage <= 0)
                model.Pager.ItemsPerPage = 10;

            model.Filter??=new FilterVM();

            Expression<Func<User, bool>> filter = u =>
            (string.IsNullOrEmpty(model.Filter.Username) || u.Username.Contains(model.Filter.Username)) &&
            (string.IsNullOrEmpty(model.Filter.FirstName) || u.FirstName.Contains(model.Filter.FirstName)) &&
            (string.IsNullOrEmpty(model.Filter.LastName) || u.LastName.Contains(model.Filter.LastName));

            model.Pager.PagesCount=(int)Math.Ceiling(repo.Count(filter)
                /(double)model.Pager.ItemsPerPage);
            model.Items=repo.GetAll(filter,u=>u.Id,model.Pager.Page,model.Pager.ItemsPerPage);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            CreateVM model = new CreateVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User item = new User();
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            if (loggedUser!=null)
                item.IsEmployee = loggedUser.IsEmployee;

            UsersRepository repo = new UsersRepository();
            repo.Save(item);

            if (loggedUser == null)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            UsersRepository repo = new UsersRepository();
            User item = repo.GetFirstOrDefault(u => u.Id == id);

            if (item == null)
                return RedirectToAction("Index", "Users");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Username = item.Username;
            model.Password = item.Password;
            model.FirstName = item.FirstName;
            model.LastName = item.LastName;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User item = new User();
            item.Id = model.Id;
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;

            UsersRepository repo = new UsersRepository();
            repo.Save(item);

            return RedirectToAction("Index", "Users");
        }

        public IActionResult Delete(int id)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            UsersRepository repo = new UsersRepository();
            User item = new User();
            item.Id = id;

            repo.Delete(item);

            return RedirectToAction("Index", "Users");
        }
    }
}
