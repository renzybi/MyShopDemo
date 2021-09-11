using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> _prodRepo;
        IRepository<ProductCategory> _prodCatRepo;

        public HomeController(
            IRepository<Product> prodRepo, 
            IRepository<ProductCategory> prodCatRepo )
        {
            _prodRepo = prodRepo;
            _prodCatRepo = prodCatRepo;
        }
        public ActionResult Index()
        {
            List<Product> products = _prodRepo.Collection().ToList();
            ViewBag.Header = "Products";

            ProductListViewModel productListViewModel = new ProductListViewModel();
            productListViewModel.Products = products;

            return View(productListViewModel);
        }

        public ActionResult Details(string id)
        {
            Product product = _prodRepo.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(product);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}