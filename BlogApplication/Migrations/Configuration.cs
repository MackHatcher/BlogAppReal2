namespace BlogApplication.Migrations
{
    using BlogApplication.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogApplication.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            // Classes to work with users and roles (provided by Microsoft packages)
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Check if the roles are already created.
            //If not, create them.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            //Check if the admin user is already created.
            //If not, create it.
            ApplicationUser adminUser = null;
            ApplicationUser modUser = null;

            if (!context.Users.Any(p => p.UserName == "admin@myblogapp.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@myblogapp.com";
                adminUser.Email = "admin@myblogapp.com";
                adminUser.FirstName = "Admin";
                adminUser.LastName = "User";
                adminUser.DisplayName = "Admin User";

                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context.Users.Where(p => p.UserName == "admin@myblogapp.com")
                    .FirstOrDefault();
            }


            if (!context.Users.Any(p => p.UserName == "mod@myblogapp.com"))
            {
                modUser = new ApplicationUser();
                modUser.UserName = "mod@myblogapp.com";
                modUser.Email = "mod@myblogapp.com";
                modUser.FirstName = "Moderator";
                modUser.LastName = "User";
                modUser.DisplayName = "Mod User";

                userManager.Create(modUser, "Password-2");
            }
            else
            {
                modUser = context.Users.Where(p => p.UserName == "mod@myblogapp.com")
                    .FirstOrDefault();
            }

            //Check if the adminUser is already on the Admin role
            //If not, add it.
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
            if (!userManager.IsInRole(modUser.Id, "Moderator"))
            {
                userManager.AddToRole(modUser.Id, "Moderator");
            }
        }
    }
}