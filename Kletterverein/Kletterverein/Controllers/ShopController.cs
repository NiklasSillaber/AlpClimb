using Kletterverein.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            //Alle Producte aus der DB lesen und mitgeben

            Product pTest1 = new Product(1, "Mein SChuhe", "Toll", Brand.Nike, 110.3m);
            Product pTest2 = new Product(2, "Deine Shu", "sad", Brand.Nike, 100.3m);

            List<Product> myProducts = new List<Product>();
            myProducts.Add(pTest1);
            myProducts.Add(pTest2);

            return View(myProducts);
        }

        public IActionResult myArticles()
        {
            return View();
        }
    }
}
