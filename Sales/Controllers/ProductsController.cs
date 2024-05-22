using Microsoft.AspNetCore.Mvc;
using Sales.Entities;
using Sales.Repositories;
using Sales.ViewModels.Shared;
using System.Linq.Expressions;
using Sales.ViewModels.Products;
using Sales.ExtensionMethods;
using System.Collections.Generic;

namespace Sales.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public IActionResult Index(IndexVM model)
        {

            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            ProductsRepository repo=new ProductsRepository();

            model.Pager ??= new PagerVM();

            if (model.Pager.Page <= 0)
                model.Pager.Page = 1;

            if (model.Pager.ItemsPerPage <= 0)
                model.Pager.ItemsPerPage = 10;

            model.Filter ??= new FilterVM();

            Expression<Func<Product, bool>> filter = u =>
            (string.IsNullOrEmpty(model.Filter.Name) || u.Name.Contains(model.Filter.Name)) &&
            (model.Filter.Quantity == 0 || u.Quantity == model.Filter.Quantity) &&
            (model.Filter.PricePerUnit == 0 || u.PricePerUnit == model.Filter.PricePerUnit);

            model.Pager.PagesCount = (int)Math.Ceiling(repo.Count(filter)
                / (double)model.Pager.ItemsPerPage);
            model.Items = repo.GetAll(filter, u => u.Id, model.Pager.Page, model.Pager.ItemsPerPage);

            return View(model);
        }

        [HttpGet]
        public IActionResult ShoppingIndex(IndexVM model)
        {

            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            ProductsRepository repo = new ProductsRepository();

            model.Pager ??= new PagerVM();

            if (model.Pager.Page <= 0)
                model.Pager.Page = 1;

            if (model.Pager.ItemsPerPage <= 0)
                model.Pager.ItemsPerPage = 10;

            model.Filter ??= new FilterVM();

            Expression<Func<Product, bool>> filter = u =>
            (string.IsNullOrEmpty(model.Filter.Name) || u.Name.Contains(model.Filter.Name)) &&
            (model.Filter.Quantity == 0 || u.Quantity == model.Filter.Quantity) &&
            (model.Filter.PricePerUnit == 0 || u.PricePerUnit == model.Filter.PricePerUnit);

            model.Pager.PagesCount = (int)Math.Ceiling(repo.Count(filter)
                / (double)model.Pager.ItemsPerPage);
            model.Items = repo.GetAll(filter, u => u.Id, model.Pager.Page, model.Pager.ItemsPerPage);

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

            Product item = new Product();
            item.Name = model.Name;
            item.Description = model.Description;
            item.Quantity = model.Quantity;
            item.PricePerUnit = model.PricePerUnit;

            ProductsRepository repo = new ProductsRepository();
            repo.Save(item);

                return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            ProductsRepository repo = new ProductsRepository();
            Product item = repo.GetFirstOrDefault(u => u.Id == id);

            if (item == null)
                return RedirectToAction("Index", "Users");

            EditVM model = new EditVM();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Description = item.Description;
            model.Quantity = item.Quantity;
            model.PricePerUnit = item.PricePerUnit;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            Product item = new Product();
            item.Id = model.Id;
            item.Name = model.Name;
            item.Description = model.Description;
            item.Quantity = model.Quantity;
            item.PricePerUnit = model.PricePerUnit;


            ProductsRepository repo = new ProductsRepository();
            repo.Save(item);

            return RedirectToAction("Index", "Products");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            ProductsRepository repo = new ProductsRepository();
            Product item = new Product();
            item.Id = id;

            repo.Delete(item);

            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            ProductsRepository repo=new ProductsRepository();
            Product item =repo.GetFirstOrDefault(x => x.Id == id);

            if (item == null)
                return RedirectToAction("ShoppingIndex", "Users");

            AddToCartVM model = new AddToCartVM();
            model.Id = item.Id;
            model.Name = item.Name;
            model.Description = item.Description;
            model.Quantity = item.Quantity;
            model.PricePerUnit = item.PricePerUnit;

            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartVM model)
        {
            if(model.Units==null)
            {
                return AddToCart(model.Id);
            }
            else
            if (model.Units <= 0)
            {
                this.ModelState.AddModelError("amountError", "The amount of units you want has to be more than 0");
                return AddToCart(model.Id);
            }
            if (model.Units>model.Quantity)
            {
                this.ModelState.AddModelError("amountError", "The amount of units you want is more than the available quantity");
                return AddToCart(model.Id);
            }
            List<AddToCartVM> cart = this.HttpContext.Session.GetObject<List<AddToCartVM>>("cart");
            if (cart.Exists(p=>p.Id==model.Id))
            {
                cart.FirstOrDefault(p => p.Id == model.Id).Units += model.Units;
            }
            else
            {
                cart.Add(model);
            }
            this.HttpContext.Session.SetObject<List<AddToCartVM>>("cart", cart);

            return RedirectToAction("ShoppingIndex", "Products");
        }

        public IActionResult RemoveFromCart(int id)
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            List<AddToCartVM> cart= this.HttpContext.Session.GetObject<List<AddToCartVM>>("cart");
            cart.RemoveAll(p => p.Id == id);
            this.HttpContext.Session.SetObject("cart",cart);
            return RedirectToAction("Create", "Sales");
        }

        public IActionResult ClearCart()
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            List<AddToCartVM> cart = this.HttpContext.Session.GetObject<List<AddToCartVM>>("cart");
            cart.Clear();
            this.HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Create", "Sales");
        }
    }
}
