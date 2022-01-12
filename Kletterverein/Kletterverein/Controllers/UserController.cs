﻿using Kletterverein.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Kletterverein.Controllers
{
    public class UserController : Controller
    {
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

        public IActionResult Login()
        {
            return View("_Message", new Message("Login", "Sie haben sich erfolgreich eingeloggt!"));
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
            ValidateRegistraionData(userDataFromForm);

            //unser Formular wurde richtig ausgefüllt
            if (ModelState.IsValid)
            {
                //TODO: Eingabedaten in einer DB speichern

                //unsere Message View aufrufen
                return View("_Message", new Message("Registrierung", "Sie haben sich erfolgreich Registriert!"));
            }

            //Das Formular wurde nicht richtig ausgefüllt
            //und die bereits eingegebenen Daten sollten wieder angezeigt werden
            return View(userDataFromForm);
        }

        private void ValidateRegistraionData(User u)
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
            //Gender

            //Birthdate
            if (u.Birthdate > DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Das Geburtsdatum darf nicht in der Zukunft sein!"); //Feld, Message
            }
            //Email
        }
    }
}
