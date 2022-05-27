using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Kletterverein.Controllers
{
    public class GetWeatherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public String GetWeatherData(String location)
        {
            //HttpClient verwenden

            

            double latitude = 0;
            double longitude = 0;
            switch (location)
            {
                case "Innsbruck": latitude = 47.259659; longitude = 11.400375; break;
                case "Arco": latitude = 37.638273; longitude = -120.907004; break;
                case "Zillertal": latitude = 47.333332; longitude = 11.8666632; break;
            }

            String urlForecast = "https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=793416b22b7fc77cbf763bbdd73b02fb";
            WebRequest request = HttpWebRequest.Create(urlForecast);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string weather = reader.ReadToEnd();
            return weather;
        }
    }
}
