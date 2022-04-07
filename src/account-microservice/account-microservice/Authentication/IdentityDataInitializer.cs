using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace account_microservice.Authentication
{
    public class IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any() || context.Roles.Any())
            {
                return;
            }

            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            if (userManager.FindByNameAsync("user").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "user";
                user.NormalizedUserName = "user";
                user.Email = "user@gmail.com";
                user.NormalizedEmail = "user@gmail.com".ToUpper();
                user.EmailConfirmed = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = userManager.CreateAsync
                (user, "Password@0").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
            if (userManager.FindByNameAsync("moderator").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "moderator";
                user.NormalizedUserName = "moderator";
                user.Email = "moderator@gmail.com";
                user.NormalizedEmail = "moderator@gmail.com".ToUpper();
                user.EmailConfirmed = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = userManager.CreateAsync
                (user, "Password@0").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Moderator").Wait();
                }
            }


            if (userManager.FindByNameAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.NormalizedUserName = "admin";
                user.Email = "admin@gmail.com";
                user.NormalizedEmail = "admin@gmail.com".ToUpper();
                user.EmailConfirmed = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                IdentityResult result = userManager.CreateAsync
                (user, "Password@1").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Moderator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Moderator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
