using account_microservice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account_microservice.Context
{
     class UserContext: DbContext
    {
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=host.docker.internal,1433;Database=UserDataBase;User ID=SA;Password=1Secure*Password1;");
        }
    }
}
