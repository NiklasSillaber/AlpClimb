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
            //User an die View übergeben
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
                    //unsere Message View aufrufen
                    return View("_Message", new Message("Ändern", "Etwas hat leider nicht geklappt!", "Bitte versuchen Sie es später erneut!"));
                }
            }
            //DbException ... Basisklasse der Datenbank-Exceptions
            catch (DbException)
            {
                //unsere Message View aufrufen
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
                    //unsere Message View aufrufen
                    return View("_Message", new Message("Löschen", "Ihr Profil konnte nicht gelöscht werden!", "Bitte versuchen Sie es später erneut!"));
                }
            }
            //DbException ... Basisklasse der Datenbank-Exceptions
            catch (DbException)
            {
                //unsere Message View aufrufen
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
            //Parameter überprüfen
            if (userDataFromForm == null)
            {
                //Weiterleitung an eine andere Action/Methode
                //im selben Controller 
                return RedirectToAction("Registration");
            }

            //Eingabedaten der Registrierung überprüfen - Validierung
            ValidateRegistrationData(userDataFromForm);

            //unser Formular wurde richtig ausgefüllt
            if (ModelState.IsValid) {
                try
                {
                    _rep.Connect();
                    if (_rep.Insert(userDataFromForm))
                    {

                        userDataFromForm.UserId = _rep.GetUserIdWithEmail(userDataFromForm.EMail);
                        //unsere Message View aufrufen
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetObject("userinfo", userDataFromForm);
                        return View("YourDataSuccess", userDataFromForm);

                    }
                    else
                    {
                        return RedirectToAction("Registration");                    
                    }
                }
                //DbException ... Basisklasse der Datenbank-Exceptions
                catch (DbException)
                {
                    //unsere Message View aufrufen
                    return View("_Message", new Message("Registrierung", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                }

                finally
                {
                    _rep.Disconnect();
                }
            }

            //Das Formular wurde nicht richtig ausgefüllt
            //und die bereits eingegebenen Daten sollten wieder angezeigt werden
            return View(userDataFromForm);
        }

        [HttpPost]
        public IActionResult Login(User userDataFromForm)
        {
            
            //Parameter überprüfen
            if (userDataFromForm == null) {
                //Weiterleitung an eine andere Action/Methode
                //im selben Controller
                isLogin = true;
                ViewBag.IsLogin = isLogin;
                return RedirectToAction("Registration");
            }

            //Eingabedaten der Registrierung überprüfen - Validierung
            ValidateLoginData(userDataFromForm);

            //unser Formular wurde richtig ausgefüllt
            if (ModelState.IsValid) {
                try
                {
                    _rep.Connect();
                    if (_rep.Login(userDataFromForm.EMail,userDataFromForm.Password))
                    {
                        //unsere Message View aufrufen
                        User a = _rep.GetUserWithEmail(userDataFromForm.EMail);
                        HttpContext.Session.Clear();
                        HttpContext.Session.SetObject("userinfo", a);
                        return View("YourDataSuccess",a);
                    }
                    else
                    {
                        isLogin = true;
                        ViewBag.IsLogin = isLogin;
                        return RedirectToAction("Registration");
                    }
                }
                //DbException ... Basisklasse der Datenbank-Exceptions
                catch (DbException)
                {
                    //unsere Message View aufrufen
                    return View("_Message", new Message("Login", "Datenbankfehler!", "Bitte versuchen Sie es später erneut!"));
                }

                finally
                {
                    _rep.Disconnect();
                }
            }

            //Das Formular wurde nicht richtig ausgefüllt
            //und die bereits eingegebenen Daten sollten wieder angezeigt werden
            isLogin = true;
            ViewBag.IsLogin = isLogin;
            return View("Registration");
        }

        private void ValidateRegistrationData(User u)
        {
            //Parameter überprüfen
            if (u == null)
            {
                return;
            }

            //Vorname
            if (u.Firstname == null)
            {
                ModelState.AddModelError("Firstname", "Bitte geben Sie einen Vornamen ein!"); //Feld, Message
            }

            //Nachname
            if (u.Lastname == null)
            {
                ModelState.AddModelError("Lastname", "Bitte geben Sie einen Nachnamen ein"); //Feld, Message
            }

            //Passwort
            if (u.Password == null || (u.Password.Length < 8))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!"); //Feld, Message

            }

            //Birthdate
            if (u.Birthdate > DateTime.Now) {
                ModelState.AddModelError("Birthdate", "Das Geburtsdatum darf nicht in der Zukunft sein!"); //Feld, Message
            }

            //Email
            if (u.EMail == null) {
                ModelState.AddModelError("EMail", "Bitte geben Sie eine Emailadresse ein!"); //Feld, Message
            }

            try
            {
                _rep.Connect();
                if (_rep.GetUserWithEmail(u.EMail) != null)
                {
                    ModelState.AddModelError("EMail", "Diese Email ist bereits registriert!"); //Feld, Message
                }
               
            }
            //DbException ... Basisklasse der Datenbank-Exceptions
            catch (DbException)
            {
                ModelState.AddModelError("EMail", "Datenbankfehler - Bitte später erneut versuchen!"); //Feld, Message
            }

            finally
            {
                _rep.Disconnect();
            }

        }



        private void ValidateLoginData(User u)
        {
            //Parameter überprüfen
            if (u == null) {
                return;
            }

            //Passwort
            if (u.Password == null || (u.Password.Length < 8)) {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!"); //Feld, Message

            }

            //Email
            if (u.EMail == null) {
                ModelState.AddModelError("EMail", "Bitte geben Sie eine Emailadresse ein!"); //Feld, Message
            }
        }
    }
}


