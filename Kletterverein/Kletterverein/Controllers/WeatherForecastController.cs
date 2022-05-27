using Kletterverein.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        [Route("allUsers")]
        public List<User> GetUsers()
        {
            return new List<User> {
                new User(){
                    UserId = 1, Birthdate = new DateTime(2020,05,18), EMail = "aaa@aaa.com", Firstname = "aaa", Gender = Gender.Männlich, Lastname = "aaaa", Password = "aaaa"
                },
                new User(){
                    UserId = 2, Birthdate = new DateTime(2020,05,18), EMail = "aaa@aaa.com", Firstname = "aaa", Gender = Gender.Männlich, Lastname = "aaaa", Password = "aaaa"
                },
                new User(){
                    UserId = 3, Birthdate = new DateTime(2020,05,18), EMail = "aaa@aaa.com", Firstname = "aaa", Gender = Gender.Männlich, Lastname = "aaaa", Password = "aaaa"
                },

            };
        }
    }
}
