namespace EnterpriseFinancialApp.Migrations
{
    using EnterpriseFinancialApp.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EnterpriseFinancialApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EnterpriseFinancialApp.Models.ApplicationDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var member = new RoleManager<IdentityRole>(store);
            var role = new IdentityRole();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                member.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                role = new IdentityRole { Name = "Member" };
                member.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "HeadOfHosehold"))
            {
                role = new IdentityRole { Name = "HeadOfHosehold" };
                member.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "tclay@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "tclay@mailinator.com",
                    Email = "tclay@mailinator.com",
                    FirstName = "Tara",
                    LastName = "Clay",
                    //AvatarPath = "",
                    //DisplayName = "TClay"
                };

                userManager.Create(user, "Abc&123!");

                userManager.AddToRoles(user.Id,
                    new string[] {
                        "HeadOfHosehold"
                    });
            }


            if (!context.Categories.Any(u => u.Name == "Utilities"))
            { context.Categories.Add(new Models.Category { Name = "Utilities" }); }
            if (!context.Categories.Any(u => u.Name == "Housing"))
            { context.Categories.Add(new Models.Category { Name = "Housing" }); }
            if (!context.Categories.Any(u => u.Name == "Food"))
            { context.Categories.Add(new Models.Category { Name = "Food" }); }
            if (!context.Categories.Any(u => u.Name == "Transportation"))
            { context.Categories.Add(new Models.Category { Name = "Transportation" }); }
            if (!context.Categories.Any(u => u.Name == "Insurance"))
            { context.Categories.Add(new Models.Category { Name = "Insurance" }); }
            if (!context.Categories.Any(u => u.Name == "Medical/Health"))
            { context.Categories.Add(new Models.Category { Name = "Medical/Health" }); }
            if (!context.Categories.Any(u => u.Name == "Savings"))
            { context.Categories.Add(new Models.Category { Name = "Savings" }); }
            if (!context.Categories.Any(u => u.Name == "Personal"))
            { context.Categories.Add(new Models.Category { Name = "Personal" }); }
            if (!context.Categories.Any(u => u.Name == "Recreation & Entertainment"))
            { context.Categories.Add(new Models.Category { Name = "Recreation & Entertainment" }); }
            if (!context.Categories.Any(u => u.Name == "Miscellaneous"))
            { context.Categories.Add(new Models.Category { Name = "Miscellaneous" }); }













            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
