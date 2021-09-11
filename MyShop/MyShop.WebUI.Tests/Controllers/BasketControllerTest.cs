using System;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Services;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            IRepository<Basket> basketRepo = new MockContext<Basket>();
            IRepository<Product> productRepo = new MockContext<Product>();
            MockHttpContext httpContext = new MockHttpContext();

            BasketService basketService = new BasketService(basketRepo, productRepo);

            basketService.AddToBasket(httpContext, "1");

            Basket basket = basketRepo.Collection().FirstOrDefault();

            Assert.IsNotNull(basket.BasketItems);
            Assert.AreEqual(1, basket.BasketItems.Count());
            Assert.AreEqual("1", basket.BasketItems.FirstOrDefault().ProductId);
        }
    }
}
