using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Data.InMem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<ProductCategory> _productCategoryRepo;

        public ProductManagerController(
            IRepository<Product> productRepo, 
            IRepository<ProductCategory> productCategoryRepo)
        {
            _productRepo = productRepo;
            _productCategoryRepo = productCategoryRepo;
        }

        public ActionResult Index()
        {
            var products = _productRepo.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerVM = new ProductManagerViewModel
            {
                Product = new Product(),
                ProductCategories = _productCategoryRepo.Collection().ToList()
            };

            return View(productManagerVM);
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
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

            ProductManagerViewModel productManagerVM = new ProductManagerViewModel
            {
                Product = product,
                ProductCategories = _productCategoryRepo.Collection().ToList()
            };

            return View(productManagerVM);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
        {
            var productToBeEdited = _productRepo.Find(id);

            if (productToBeEdited == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    if (file != null)
                    {
                        productToBeEdited.Image = productToBeEdited.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToBeEdited.Image);
                    }

                    productToBeEdited.Name = product.Name;
                    productToBeEdited.Category = product.Category;
                    productToBeEdited.Description = product.Description;
                    productToBeEdited.Price = product.Price;

                    _productRepo.Update(productToBeEdited);
                    _productRepo.Commit();

                    return RedirectToAction("Index");
                }
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