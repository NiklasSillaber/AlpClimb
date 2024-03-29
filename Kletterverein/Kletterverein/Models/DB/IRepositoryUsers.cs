﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kletterverein.Models.DB
{
    interface IRepositoryUsers
    {
        void Connect();
        void Disconnect();

        bool Insert(User user);
        bool Delete(int userId);
        bool Update(User newUserData);
        User GetUser(int userId);
        User GetUserWithEmail(String email);
        public int GetUserIdWithEmail(String email);
        bool Login(string username, string password);
    }
}
