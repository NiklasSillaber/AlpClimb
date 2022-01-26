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
        public IActionResult Index()
        {
            // Intsanz/Objekt von User erzeugen
            //Objektinitialisierersyntax
            User user = new User()
            {
                UserId = 100,
                Firstname = "Daniel",
                Lastname = "Daniel",
                Password = "Sesam123"
            };

            //User an die View übergeben
            return View(user);
        }

        public IActionResult YourData()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User userDataFromForm)
        {

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
                        //unsere Message View aufrufen
                        return View("YourData");
                    }
                    else
                    {
                        //unsere Message View aufrufen
                        return View("_Message", new Message("Registrierung", "Sie haben sich nicht erfolgreich registriert!", "Bitte versuchen Sie es später erneut!"));
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
                return RedirectToAction("Login");
            }

            //Eingabedaten der Registrierung überprüfen - Validierung
            ValidateLoginData(userDataFromForm);

            //unser Formular wurde richtig ausgefüllt
            if (ModelState.IsValid) {
                //TODO: Eingabedaten in DB überprüfen

                //unsere Message View aufrufen
                return View("_Message", new Message("Login", "Sie haben sich erfolgreich Eingeloggt!"));
            }

            //Das Formular wurde nicht richtig ausgefüllt
            //und die bereits eingegebenen Daten sollten wieder angezeigt werden
            return View(userDataFromForm);
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
