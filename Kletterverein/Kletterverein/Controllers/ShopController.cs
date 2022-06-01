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
        public async Task<IActionResult> Index()
        {
            //Alle Producte aus der DB lesen und mitgeben
            try
            {
                await _rep.ConnectAsync();

                return View(await _rep.GetProductsAsync());
            }
            catch (DbException)
            {
                return View("_Message", new Message("Shop", "Datenbankfehler", "Bitte versuchen sie es später erneut!"));

            }
            finally
            {
                await _rep.DisconnectAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart()
        {
            try
            {
                await _rep.ConnectAsync();

                User a = HttpContext.Session.GetObject("userinfo");

                if (a != null)
                {
                    if(!await _rep.productAlreadyInCartAsync(a.UserId, productIdAddToCart)) {
                        if (await _rep.addProductToCartAsync(a.UserId, productIdAddToCart)) {
                            return RedirectToAction("MyArticles");
                        } else {
                            return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                        }
                    } else {
                        return View("_Message", new Message("Produkt hinzufügen", "Datenbankfehler!", "Produkt ist schon im Warenkorb!"));
                    }


                }

                return RedirectToAction("Registration", "User");
            }
            catch (DbException)
            {
                return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                await _rep.DisconnectAsync();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Product p)
        {
            try
            {
                await _rep.ConnectAsync();

                User a = HttpContext.Session.GetObject("userinfo");

                if(a != null)
                {
                    if(!await _rep.productAlreadyInCartAsync(a.UserId, p.ProductId)) {
                        if (await _rep.addProductToCartAsync(a.UserId, p.ProductId)) {
                            return RedirectToAction("MyArticles");
                        }
                        else {
                            return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                        }
                        
                    }
                    else {
                        return View("_Message", new Message("Produkt hinzufügen", "Datenbankfehler!", "Produkt ist schon im Warenkorb!"));
                    }
                    
                }
                
                duringAddToCart = true;
                productIdAddToCart = p.ProductId;
                return RedirectToAction("Registration", "User");


            }
            catch (DbException)
            {

                return View("_Message", new Message("AddToCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));

            }
            finally
            {
                await _rep.DisconnectAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyArticles()
        {
            try
            {
                await _rep.ConnectAsync();

                User a = HttpContext.Session.GetObject("userinfo");

                if (a != null)
                {
                    List<Product> prodList = await _rep.getProductsOfCartAsync(a.UserId);
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
                await _rep.DisconnectAsync();
            }
            
                
        }

        public async Task<IActionResult> DeleteFromCart(int prodId)
        {
            try
            {
                await _rep.ConnectAsync();

                User a = HttpContext.Session.GetObject("userinfo");

                bool result = await _rep.deleteProductFromCartAsync(a.UserId,prodId);
                
                return RedirectToAction("MyArticles");

            }
            catch (DbException)
            {
                return View("_Message", new Message("DeleteFromCart", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }
            finally
            {
                await _rep.DisconnectAsync();
            }
            
        }
    }
}
