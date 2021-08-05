using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

using YourPlace.Data.Data;
using YourPlace.Models.Models;

using static YourPlace.Web.Areas.Admin.AdminConstants;

namespace YourPlace.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();

            data.Database.Migrate();

            Seed(data);
            SeedAdmin(scopedServices.ServiceProvider);

            return app;
        }

        private static void Seed(ApplicationDbContext data)
        {

        }

        private static void SeedAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminUsername = "admin";
                    const string adminEmail = "admin@yourplace.com";
                    const string adminPassword = "admin12";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminUsername
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}

