using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models
{
    public class User
    {
        private int _userId;

        public int UserId
        {
            get { return this._userId; }
            set { if(value >= 0) {
                    this._userId = value;
                  } 
            }
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }

        public DateTime Birthdate { get; set; }
        public string EMail { get; set; }
        public Gender Gender { get; set; }

        public String toString() {
            return this._userId + " " + this.Firstname + " " + this.Lastname + " " + this.Password + " " + this.Birthdate + " " + this.EMail +
                " " + this.Gender;
        }
    }
}
