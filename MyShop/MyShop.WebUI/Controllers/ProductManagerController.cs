using MyShop.Core.Models;
using MyShop.Data.InMem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly ProductRepository _productRepo;

        public ProductManagerController()
        {
            _productRepo = new ProductRepository();
        }

        public ActionResult Index()
        {
            var products = _productRepo.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                _productRepo.Insert(product);
                _productRepo.Commit();
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(string id)
        {
            Product product = _productRepo.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                _productRepo.Update(product);
                _productRepo.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string id)
        {
            var productToBeDelete = _productRepo.Find(id);

            if (productToBeDelete == null)
            {
                return HttpNotFound();
            }

            else
            {
                return View(productToBeDelete);
            }
        }
        
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToBeDelete = _productRepo.Find(id);

            if (productToBeDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                _productRepo.Delete(id);
                _productRepo.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}