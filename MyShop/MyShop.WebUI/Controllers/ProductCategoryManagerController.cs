using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Data.InMem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        private readonly IRepository<ProductCategory> _prodCatRepo;

        public ProductCategoryManagerController(IRepository<ProductCategory> prodCatRepo)
        {
            _prodCatRepo = prodCatRepo;
        }

        public ActionResult Index()
        {
            List<ProductCategory> prodCategories = _prodCatRepo.Collection().ToList();
            return View(prodCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                _prodCatRepo.Insert(productCategory);
                _prodCatRepo.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string id)
        {
            ProductCategory productCategoryToBeEdited = _prodCatRepo.Find(id);

            if (productCategoryToBeEdited == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToBeEdited);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory productCategoryToBeEdited = _prodCatRepo.Find(id);

            if (productCategoryToBeEdited == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                else
                {
                    productCategoryToBeEdited.CategoryName = productCategory.CategoryName;

                    _prodCatRepo.Update(productCategoryToBeEdited);
                    _prodCatRepo.Commit();

                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(string id)
        {
            ProductCategory productCategoryToBeDeleted = _prodCatRepo.Find(id);

            if (productCategoryToBeDeleted == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToBeDeleted);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            ProductCategory productCategoryToBeDeleted = _prodCatRepo.Find(id);

            if (productCategoryToBeDeleted == null)
            {
                return HttpNotFound();
            }
            else
            {
                _prodCatRepo.Delete(id);
                _prodCatRepo.Commit();

                return RedirectToAction("Index");
            }
        }
    }
}