﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RockyMvcCoreWebApp;
using RockyMvcCoreWebApp.Data;
using RockyMvcCoreWebApp_Models;
using RockyMvcCoreWebApp_Models.ViewModel;
using RockyMvcCoreWebApp_Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType),
                Categories = _db.Category
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType).Where(x => x.Id == id).FirstOrDefault(),
                ExistInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistInCart = true;
                }
            }
            return View(detailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            var itemTORemove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if (itemTORemove != null)
            {
                shoppingCartList.Remove(itemTORemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
