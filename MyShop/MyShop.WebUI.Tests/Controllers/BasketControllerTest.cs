using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
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
        public void CanGetSummaryViewModel()
        {
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();

            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(baskets, products);
            IOrderService orderService = new OrderService(orders);
            
            var controller = new BasketController(basketService, orderService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var viewModel = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(3, viewModel.BasketCount);
            Assert.AreEqual(25.00m, viewModel.BasketTotal);
        }

        [TestMethod]
        public void CanCheckOutAndCreateOrder()
        {
            //Arrange
            IRepository<Product> prodRepo = new MockContext<Product>();
            prodRepo.Insert(new Product { Id = "1", Price = 10.00m });
            prodRepo.Insert(new Product { Id = "2", Price = 20.00m });

            IRepository<Basket> basketRepo = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem { BasketId=basket.Id, ProductId="1", Quantity=2 });
            basket.BasketItems.Add(new BasketItem { BasketId = basket.Id, ProductId = "2", Quantity = 1 });
            basketRepo.Insert(basket);

            IRepository<Order> orderRepo = new MockContext<Order>();
            
            IOrderService orderService = new OrderService(orderRepo);
            IBasketService basketService = new BasketService(basketRepo, prodRepo);
            
            MockHttpContext httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            BasketController controller = new BasketController(basketService, orderService);
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order();
            controller.CheckOut(order);

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRep = orderRepo.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);
        }
    }
}
