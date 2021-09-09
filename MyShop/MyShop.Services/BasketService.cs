using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Basket> _basketRepo;
        IRepository<Product> _prodRepo;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Basket> basketRepo, IRepository<Product> prodRepo)
        {
            _basketRepo = basketRepo;
            _prodRepo = prodRepo;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;

                if (string.IsNullOrEmpty(basketId))
                {
                    basket = _basketRepo.Find(basketId);
                }

                else
                {
                    if (createIfNull)
                    {
                        basket = CreateBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();

            _basketRepo.Insert(basket);
            _basketRepo.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (basketItem != null)
            {
                basketItem.Quantity = basketItem.Quantity + 1;
            }

            else
            {
                basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(basketItem);
            }

            _basketRepo.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);

            BasketItem basketItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (basketItem != null)
            {
                basket.BasketItems.Remove(basketItem);
                _basketRepo.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            
            if (basket != null)
            {
                var result = (from b in basket.BasketItems
                              join p in _prodRepo.Collection() on b.ProductId equals p.Id
                              select new BasketItemViewModel
                              {
                                  Id = b.Id,
                                  Quantity = b.Quantity,
                                  ProductName = p.Name,
                                  Price = p.Price,
                                  Image = p.Image
                              }).ToList();

                return result;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            var basketSummaryVM = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from b in basket.BasketItems
                                        join p in _prodRepo.Collection() on b.ProductId equals p.Id
                                        select b.Quantity * p.Price).Sum();

                basketSummaryVM.BasketCount = basketCount ?? 0;
                basketSummaryVM.BasketTotal = basketTotal ?? decimal.Zero;

                return basketSummaryVM;
            }

            else
            {
                return basketSummaryVM;
            }
        }
    }
}
