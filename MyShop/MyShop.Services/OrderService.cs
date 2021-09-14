using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        private IRepository<Order> _orderRepo;

        public OrderService(IRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Image = item.Image,
                    Quantity = item.Quantity
                });
            }
            _orderRepo.Insert(baseOrder);
            _orderRepo.Commit();
        }
    }
}
