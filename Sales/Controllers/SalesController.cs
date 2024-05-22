using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sales.Entities;
using Sales.Repositories;
using Sales.ViewModels.Shared;
using Sales.ViewModels.Sales;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sales.ExtensionMethods;

namespace Sales.Controllers
{
    public class SalesController : Controller
    {
        [HttpGet]
        public IActionResult Index(IndexVM model)
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            SalesRepository repo = new SalesRepository();

            model.Pager ??= new PagerVM();

            if (model.Pager.Page <= 0)
                model.Pager.Page = 1;

            if (model.Pager.ItemsPerPage <= 0)
                model.Pager.ItemsPerPage = 10;

            model.Filter ??= new FilterVM();

            Expression<Func<Sale, bool>> filter = s =>
            (string.IsNullOrEmpty(model.Filter.Status.ToString()) || s.Status == model.Filter.Status) &&
            (model.Filter.Date == DateTime.MinValue || s.Date.Date == model.Filter.Date.Date)
            && this.HttpContext.Session.GetObject<User>("loggedUser").Id == s.UserId;

            model.Pager.PagesCount = (int)Math.Ceiling(repo.Count(filter)
                / (double)model.Pager.ItemsPerPage);
            model.Items = repo.GetAll(filter, u => u.Id, model.Pager.Page, model.Pager.ItemsPerPage);

            return View(model);
        }

        [HttpGet]
        public IActionResult PendingIndex(IndexVM model)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            SalesRepository salesRepo = new SalesRepository();

            model.Pager ??= new PagerVM();

            if (model.Pager.Page <= 0)
                model.Pager.Page = 1;

            if (model.Pager.ItemsPerPage <= 0)
                model.Pager.ItemsPerPage = 10;

            model.Filter ??= new FilterVM();

            Expression<Func<Sale, bool>> filter = u =>
            (model.Filter.UserId <= 0 || model.Filter.UserId == u.UserId) &&
            (model.Filter.Date == DateTime.MinValue || u.Date.Date == model.Filter.Date.Date)
            && u.Status==Enums.Status.Pending;

            model.Pager.PagesCount = (int)Math.Ceiling(salesRepo.Count(filter)
                / (double)model.Pager.ItemsPerPage);
            model.Items = salesRepo.GetAll(filter, u => u.Id, model.Pager.Page, model.Pager.ItemsPerPage);

            UsersRepository usersRepo= new UsersRepository();
            model.Filter.Users = usersRepo.GetAll()
                .Select(u=>new SelectListItem()
                    {
                    Text = $"{u.FirstName} {u.LastName} ({u.Username})",
                    Value = u.Id.ToString(),
                    Selected = u.Id == model.Filter.UserId
                    }
                );

            return View(model);
        }

        [HttpPut]
        public IActionResult Approve(int id)
        {
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            SalesRepository repo=new SalesRepository();
            Sale sale=repo.GetFirstOrDefault(s=>s.Id==id);
            sale.Status= Enums.Status.Approved;
            repo.Save(sale);
            return RedirectToAction("PendingIndex", "Sales");
        }

        [HttpPut]
        public IActionResult Cancel(int id)
        {
            SalesRepository saleRepo = new SalesRepository();
            Sale sale = saleRepo.GetFirstOrDefault(s => s.Id == id);
            if (!this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee
                && this.HttpContext.Session.GetObject<User>("loggedUser").Id !=sale.UserId)
                return RedirectToAction("Index", "Home");
            sale.Status = Enums.Status.Cancelled;
            saleRepo.Save(sale);
            SaleProductsRepository saleProductsRepo=new SaleProductsRepository();
            ProductsRepository productsRepo=new ProductsRepository();
            foreach (SaleProduct saleProduct in saleProductsRepo.GetAll(x=>x.SaleId==sale.Id))
            {
                Product product = productsRepo.GetFirstOrDefault(p => p.Id == saleProduct.Id);
                product.Quantity += saleProduct.Quantity;
                productsRepo.Save(product);
            }
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
            {
                return RedirectToAction("PendingIndex", "Sales");
            }
            return RedirectToAction("Index", "Sales");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (this.HttpContext.Session.GetObject<User>("loggedUser").IsEmployee)
                return RedirectToAction("Index", "Home");
            CreateVM model = new CreateVM();
            model.SaleProducts = this.HttpContext.Session.GetObject<List<ViewModels.Products.AddToCartVM>>("cart");
            foreach (ViewModels.Products.AddToCartVM saleProduct in model.SaleProducts)
            {
                model.TotalPrice += saleProduct.Units * saleProduct.PricePerUnit;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateVM model)
        {
            model.SaleProducts= this.HttpContext.Session.GetObject<List<ViewModels.Products.AddToCartVM>>("cart");
            if (model.SaleProducts.Count<1)
            {
                this.ModelState.AddModelError("noProductsError", "There are no products in your cart!");
                return View(model);
            }
            SalesRepository salesRepo = new SalesRepository();
            Sale sale=new Sale();
            sale.Status=Enums.Status.Pending;
            sale.Date = DateTime.Now;
            sale.UserId = this.HttpContext.Session.GetObject<User>("loggedUser").Id;
            salesRepo.Save(sale);

            SaleProductsRepository saleProductsRepository =new SaleProductsRepository();
            ProductsRepository productsRepo = new ProductsRepository();
            foreach (ViewModels.Products.AddToCartVM saleProduct in model.SaleProducts)
            {
                SaleProduct item=new SaleProduct();
                item.ProductId = saleProduct.Id;
                item.SaleId= sale.Id;
                item.Quantity= saleProduct.Units;
                saleProductsRepository.Save(item);
                Product product = productsRepo.GetFirstOrDefault(p=>p.Id==saleProduct.Id);
                product.Quantity-=item.Quantity;
                productsRepo.Save(product);
            }

            return RedirectToAction("Index", "Sales");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            DetailsVM model = new DetailsVM();
            SalesRepository salesRepo=new SalesRepository();
            Sale sale=salesRepo.GetFirstOrDefault(s=>s.Id==id);
            if (this.HttpContext.Session.GetObject<User>("loggedUser").Id!=sale.UserId)
                return RedirectToAction("Index", "Home");
            model.Id= sale.Id;
            model.Status= sale.Status;
            model.Date= sale.Date;
            model.SaleProducts = new List<ItemVM>();
            SaleProductsRepository saleProductsRepo=new SaleProductsRepository();
            ProductsRepository productsRepo = new ProductsRepository();
            foreach (SaleProduct saleProduct in saleProductsRepo.GetAll(x=>x.SaleId==id))
            {
                ItemVM item=new ItemVM();
                item.Quantity= saleProduct.Quantity;
                Product product = productsRepo.GetFirstOrDefault(p => p.Id == saleProduct.ProductId);
                item.Name=product.Name;
                item.PricePerUnit=product.PricePerUnit;
                model.SaleProducts.Add(item);
            }
            foreach (ItemVM item in model.SaleProducts)
            {
                model.TotalPrice += item.Quantity * item.PricePerUnit;
            }
            return View(model);
        }
    }
}
