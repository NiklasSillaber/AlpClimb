using Kletterverein.Models;
using Kletterverein.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Common;

namespace Kletterverein.Controllers
{
    public class UserController : Controller
    {
        private IRepositoryUsers _rep = new RepositoryUsersDB();
        bool isLogin = false;
        public IActionResult Index()
        {
            User a = HttpContext.Session.GetObject("userinfo");

            if (a != null)
            {
                return RedirectToAction("YourData");
            }
            return View();
        }

        [HttpGet]
        public IActionResult YourData()
        {
            User sessionUser = HttpContext.Session.GetObject("userinfo");
            return View(sessionUser);
        }

        [HttpPost]
        public IActionResult YourData(User UserDataUpdated)
        {
            User sessionUser = HttpContext.Session.GetObject("userinfo");
            UserDataUpdated.UserId = sessionUser.UserId;
            UserDataUpdated.EMail = sessionUser.EMail;
            HttpContext.Session.Clear();
            try
            {
                _rep.Connect();
                if (_rep.Update(UserDataUpdated))
                {
                    return RedirectToAction("Registration");
                }
                else
                {
                    return View("_Message", new Message("Ändern", "Etwas hat leider nicht geklappt!", "Bitte versuchen Sie es später erneut!"));
                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Ändern", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }

            finally
            {
                _rep.Disconnect();
            }
        }

        public IActionResult YourData_Delete()
        {
            User sessionUser = HttpContext.Session.GetObject("userinfo");
            HttpContext.Session.Clear();
            try
            {
                _rep.Connect();
                if (_rep.Delete(sessionUser.UserId))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("_Message", new Message("Löschen", "Ihr Profil konnte nicht gelöscht werden!", "Bitte versuchen Sie es später erneut!"));
                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Löschen", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
            }

            finally
            {
                _rep.Disconnect();
            }
            
        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User userDataFromForm)
        {
            isLogin = false;
            ViewBag.IsLogin = isLogin;
            if (userDataFromForm == null)
            {
                return RedirectToAction("Registration");
            }

            ValidateRegistrationData(userDataFromForm);

            if (ModelState.IsValid) {
                try
                {
                    _rep.Connect();
                    if (_rep.Insert(userDataFromForm))
                    {

                        userDataFromForm.UserId = _rep.GetUserIdWithEmail(userDataFromForm.EMail);
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetObject("userinfo", userDataFromForm);
                        if (ShopController.registrationOnShopping)
                        {
                            ShopController.registrationOnShopping = false;
                            return RedirectToAction("MyArticles","Shop");
                        }
                        if (ShopController.duringAddToCart)
                        {
                            ShopController.duringAddToCart = false;
                            return RedirectToAction("AddToCart", "Shop");
                        }
                        return View("YourDataSuccess", userDataFromForm);

                    }
                    else
                    {
                        return RedirectToAction("Registration");                    
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Registrierung", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                }

                finally
                {
                    _rep.Disconnect();
                }
            }

            return View(userDataFromForm);
        }

        [HttpPost]
        public IActionResult Login(User userDataFromForm)
        {
            
            if (userDataFromForm == null) {
                isLogin = true;
                ViewBag.IsLogin = isLogin;
                return RedirectToAction("Registration");
            }

            ValidateLoginData(userDataFromForm);

            if (ModelState.IsValid) {
                try
                {
                    _rep.Connect();
                    if (_rep.Login(userDataFromForm.EMail,userDataFromForm.Password))
                    {
                        User a = _rep.GetUserWithEmail(userDataFromForm.EMail);
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetObject("userinfo", a);
                        if (ShopController.registrationOnShopping)
                        {
                            ShopController.registrationOnShopping = false;
                            return RedirectToAction("MyArticles", "Shop");
                        }
                        if (ShopController.duringAddToCart)
                        {
                            ShopController.duringAddToCart = false;
                            return RedirectToAction("AddToCart", "Shop");
                        }
                        return View("YourDataSuccess",a);
                    }
                    else
                    {
                        isLogin = true;
                        ViewBag.IsLogin = isLogin;
                        return RedirectToAction("Registration");
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Login", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                }

                finally
                {
                    _rep.Disconnect();
                }
            }

            isLogin = true;
            ViewBag.IsLogin = isLogin;
            return View("Registration");
        }

        private void ValidateRegistrationData(User u)
        {
            if (u == null)
            {
                return;
            }

            if (u.Firstname == null)
            {
                ModelState.AddModelError("Firstname", "Bitte geben Sie einen Vornamen ein!");
            }

            if (u.Lastname == null)
            {
                ModelState.AddModelError("Lastname", "Bitte geben Sie einen Nachnamen ein");
            }

            if (u.Password == null || (u.Password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!");

            }

            if (u.Birthdate > DateTime.Now) {
                ModelState.AddModelError("Birthdate", "Das Geburtsdatum darf nicht in der Zukunft sein!");
            }

            if (u.EMail == null) {
                ModelState.AddModelError("EMail", "Bitte geben Sie eine Emailadresse ein!");
            }

            try
            {
                _rep.Connect();
                if (_rep.GetUserWithEmail(u.EMail) != null)
                {
                    ModelState.AddModelError("EMail", "Diese Email ist bereits registriert!");
                }
               
            }
            catch (DbException)
            {
                ModelState.AddModelError("EMail", "Datenbankfehler - Bitte später erneut versuchen!");
            }

            finally
            {
                _rep.Disconnect();
            }

        }



        private void ValidateLoginData(User u)
        {
            if (u == null) {
                return;
            }

            if (u.Password == null || (u.Password.Length < 8)) {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!");

            }

            if (u.EMail == null) {
                ModelState.AddModelError("EMail", "Bitte geben Sie eine Emailadresse ein!");
            }
        }
    }
}


