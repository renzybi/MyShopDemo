using System;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Services;
using MyShop.WebUI.Controllers;
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

        [TestMethod]
        public void CanGetBasketSummaryViewModel()
        {
            IRepository<Basket> basketRepo = new MockContext<Basket>();
            IRepository<Product> prodRepo = new MockContext<Product>();
            MockHttpContext httpContext = new MockHttpContext();

            prodRepo.Insert(new Product() { Id = "1", Price = 100.00m });
            prodRepo.Insert(new Product() { Id = "2", Price = 50.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId="1", Quantity=2 });
            basket.BasketItems.Add(new BasketItem() { ProductId="2", Quantity=1 });
            basketRepo.Insert(basket);
            
            BasketService basketService = new BasketService(basketRepo, prodRepo);

            var controller = new BasketController(basketService);
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
        }
    }
}
