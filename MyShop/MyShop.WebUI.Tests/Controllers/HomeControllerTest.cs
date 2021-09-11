using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> prodRepo = new MockContext<Product>();
            IRepository<ProductCategory> prodCatRepo = new MockContext<ProductCategory>();

            prodRepo.Insert(new Product());

            HomeController homeController = new HomeController(prodRepo, prodCatRepo);
            
            var result = homeController.Index() as ViewResult;
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());
        }

        //[TestMethod]
        //public void About()
        //{
            
        //}

        //[TestMethod]
        //public void Contact()
        //{
            
        //}
    }
}
