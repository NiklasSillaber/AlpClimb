using Kletterverein.Models;
using Kletterverein.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Controllers
{
    public class ShopController : Controller
    {
        private IRepositoryShop _rep = new RepositoryShopDB();

        //für die Weiterleitung zur Anmeldung wenn man etwas kaufen möchte und noch nicht angemeldet ist
        public static bool registrationOnShopping = false;
        public static bool duringAddToCart = false;
        public static int productIdAddToCart = 0;

        [HttpGet]
        public IActionResult Index()
        {
            //Alle Producte aus der DB lesen und mitgeben
            try
            {
                _rep.Connect();

                return View(_rep.GetProducts());
            }
            catch (DbException)
            {
                return View("_Message", new Message("Shop", "Datenbankfehler", "Bitte versuchen sie es später erneut!"));

            }
            finally
            {
                _rep.Disconnect();
            }
        }

        [HttpGet]
        public IActionResult AddToCart()
        {
            try
            {
                _rep.Connect();

                User a = HttpContext.Session.GetObject("userinfo");

                if (a != null)
                {
                    bool c = _rep.addProductToCart(a.UserId, productIdAddToCart);
                    return RedirectToAction("MyArticles");
                }

                return RedirectToAction("Registration", "User");
            }
            catch (DbException)
            {
                return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
        }

        [HttpPost]
        public IActionResult AddToCart(Product p)
        {
            try
            {
                _rep.Connect();

                User a = HttpContext.Session.GetObject("userinfo");

                if(a != null)
                {
                    bool c = _rep.addProductToCart(a.UserId, p.ProductId);
                    return RedirectToAction("MyArticles");
                }
                
                duringAddToCart = true;
                productIdAddToCart = p.ProductId;
                return RedirectToAction("Registration","User");


            }
            catch (DbException)
            {

                return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));

            }
            finally
            {
                _rep.Disconnect();
            }
        }

        [HttpGet]
        public IActionResult MyArticles()
        {
            try
            {
                _rep.Connect();

                User a = HttpContext.Session.GetObject("userinfo");

                if (a != null)
                {
                    List<Product> prodList = _rep.getProductsOfCart(a.UserId);
                    return View(prodList);
                }

                registrationOnShopping = true;
                return RedirectToAction("Registration","User");
            }
            catch (DbException)
            {
                return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
            
                
        }

        public IActionResult DeleteFromCart(int prodId)
        {
            try
            {
                _rep.Connect();

                User a = HttpContext.Session.GetObject("userinfo");

                bool result = _rep.deleteProductFromCart(a.UserId,prodId);
                
                return RedirectToAction("MyArticles");

            }
            catch (DbException)
            {
                return View("_Message", new Message("DeleteFromCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                _rep.Disconnect();
            }
            
        }
    }
}
