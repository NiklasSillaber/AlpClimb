using Kletterverein.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Controllers
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, User value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static User GetObject(this ISession session, string key)
        {
            var value = session.GetString(key);
            if(value == null)
            {
                return null;
            }
            User a = JsonConvert.DeserializeObject<User>(value);
            return a;
        }
    }
}
