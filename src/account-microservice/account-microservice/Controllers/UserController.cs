using account_microservice.Context;
using account_microservice.Models;
using account_microservice.Tools;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace account_microservice.Controllers
{
     class UserController
    {
        private PasswordHasher hasher;
        private UserContext userContext;
        public UserController(PasswordHasher hasher, UserContext userContext)
        {
            this.hasher = hasher;
            this.userContext = userContext;
        }
        public async Task<Response> AddUser(string name, string email, string password)
        {


            User existsn = await userContext.users.Where(p=>p.Name == name).FirstAsync();
            if (existsn != null)
            {
                return new Response("Failed","Username already exists",false);
            }
            User existsm = await userContext.users.Where(p => p.Email == email).FirstAsync();
            if (existsm != null)
            {
                return new Response("Failed", "Email already exists", false);
            }
            User userToAdd = new User()
            {
                Name = name,
                Email = email,
                Password = hasher.HashPassword(password).ToString(),
                CreatedDate = DateTime.Now,
            };
            await userContext.users.AddAsync(userToAdd);
            await userContext.SaveChangesAsync();
            return new Response("Success", "Account created sucsesfully", true);
        }

    }
}
