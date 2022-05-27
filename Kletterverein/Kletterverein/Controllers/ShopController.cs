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

            Product pTest1 = new Product(1, "SCHU", "Toll", Brand.Nike, 110.3m);
            Product pTest2 = new Product(2, "Deine Shu", "sad", Brand.Nike, 39.3m);
            Product pTest3 = new Product(3, "Mein SChuhe", "Toll", Brand.Nike, 110.3m);
            Product pTest4 = new Product(4, "Deine Shu", "sad", Brand.Nike, 100.3m);
            Product pTest5 = new Product(5, "Mein SChuhe", "Toll", Brand.Nike, 110.3m);
            Product pTest6 = new Product(6, "Deine Shu", "sad", Brand.Nike, 100.3m);
            Product pTest7 = new Product(7, "Mein SChuhe", "Toll", Brand.Nike, 110.3m);
            Product pTest8 = new Product(8, "Deine Shu", "sad", Brand.Nike, 100.3m);
            Product pTest9 = new Product(9, "Mein SChuhe", "Toll", Brand.Nike, 110.3m);
            Product pTest10 = new Product(10, "Deine Shu", "sad", Brand.Nike, 100.3m);

            List<Product> myProducts = new List<Product>();
            myProducts.Add(pTest1);
            myProducts.Add(pTest2);
            myProducts.Add(pTest3);
            myProducts.Add(pTest4);
            myProducts.Add(pTest5);
            myProducts.Add(pTest6);
            myProducts.Add(pTest7);
            myProducts.Add(pTest8);
            myProducts.Add(pTest9);
            myProducts.Add(pTest10);

            return View(myProducts);
        }

        public IActionResult ShoppingCart()
        {
            return View();
        }
    }
}
