using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService _basketService;
        // GET: Basket
        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }
        public ActionResult Index()
        {
            List<BasketItemViewModel> basketVM = _basketService.GetBasketItems(this.HttpContext);
            return View(basketVM);
        }
    }
}