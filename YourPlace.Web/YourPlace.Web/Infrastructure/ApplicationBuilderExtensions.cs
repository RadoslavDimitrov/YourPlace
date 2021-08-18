using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

using YourPlace.Data.Data;
using YourPlace.Models.Models;

using static YourPlace.Web.Infrastructure.RoleConstants;
using static YourPlace.Web.Areas.Admin.AdminConstants;
using System.Collections.Generic;

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

            SeedAdminAndUsers(scopedServices.ServiceProvider);
            Seed(data);

            return app;
        }

        private static void Seed(ApplicationDbContext data)
        {
            var userRado = data.Users.Where(u => u.UserName == "Rado").FirstOrDefault();
            var userRadoStore = data.Users.Where(u => u.UserName == "Rado").Select(u => u.StoreId).FirstOrDefault();

            if(userRadoStore == null)
            {
                var district = new District
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Drujba"
                };
                var town = new Town
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Sofia"
                };

                town.Districts.Add(district);

                userRado.Store = new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Auto dream",
                    Type = "Mechanic",
                    Description = "Auto mechanic",
                    OpenHour = 9,
                    CloseHour = 18,
                    PictureUrl = "https://miro.medium.com/max/4320/1*JktzC9GrA_l4yz0cCy8a5Q.jpeg",
                    Town = town,
                    District = district,
                };
            }

            var userPesho = data.Users.Where(u => u.UserName == "Pesho").FirstOrDefault();
            var userPeshoStore = data.Users.Where(u => u.UserName == "Pesho").Select(u => u.StoreId).FirstOrDefault();

            if (userPeshoStore == null)
            {
                var district = new District
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Izgrev"
                };
                var town = new Town
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Plovdiv"
                };

                town.Districts.Add(district);

                userPesho.Store = new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Cosmetic beauty",
                    Type = "Cosmetics",
                    Description = "Perfect place for your hands!",
                    OpenHour = 9,
                    CloseHour = 20,
                    PictureUrl = "https://goodspaguide.co.uk/images/uploads/Hand-with-french-manicure.jpg",
                    Town = town,
                    District = district,
                };

                userPesho.Store.StoreServices.Add(new StoreServices
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "manicure",
                    Price = 25,
                    Description = "standart manicure",
                    Store = userPesho.Store
                });
            }

            var userGosho = data.Users.Where(u => u.UserName == "Gosho").FirstOrDefault();
            var userGoshoStore = data.Users.Where(u => u.UserName == "Gosho").Select(u => u.StoreId).FirstOrDefault();

            if (userGoshoStore == null)
            {
                var district = new District
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Levski"
                };

                var town = new Town
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Varna"
                };

                town.Districts.Add(district);

                userGosho.Store = new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Best place",
                    Type = "lawyer",
                    Description = "Perfect man for your job!",
                    OpenHour = 9,
                    CloseHour = 17,
                    PictureUrl = "https://thumbor.forbes.com/thumbor/960x0/https%3A%2F%2Fspecials-images.forbesimg.com%2Fdam%2Fimageserve%2F522936350%2F960x0.jpg%3Ffit%3Dscale",
                    Town = town,
                    District = district,
                };

                List<int> ratings = new List<int>() { 5, 8, 9, 4, 6 };

                var userId = data.Users.Where(u => u.UserName == "Rado").Select(u => u.Id).FirstOrDefault();

                foreach (var rating in ratings)
                {
                    userGosho.Store.Raitings.Add(new Rating
                    {
                        Id = Guid.NewGuid().ToString(),
                        StoreRaiting = rating,
                        Store = userGosho.Store,
                        UserId = userId
                    });
                }
            }

            data.SaveChanges();
        }


        private static void SeedAdminAndUsers(IServiceProvider services)
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

            Task
                .Run(async () =>
                {
                    var UsersToAdd = new List<string>() { "Rado", "Pesho", "Gosho", "Kiro" };

                    var data = services.GetService<ApplicationDbContext>();

                    foreach (var userToAdd in UsersToAdd)
                    {
                        if(data.Users.Where(u => u.UserName == userToAdd).FirstOrDefault() != null)
                        {
                            break;
                        }

                        var user = new User
                        {
                            UserName = userToAdd,
                            Email = userToAdd + $"@{userToAdd}.bg"
                        };

                        var currUserPassword = userToAdd + "12";

                        var result = await userManager.CreateAsync(user, currUserPassword);

                        if (await roleManager.RoleExistsAsync(UserRoleName))
                        {
                            await userManager.AddToRoleAsync(user, UserRoleName);
                        }
                        else
                        {
                            var role = new IdentityRole { Name = UserRoleName };

                            await roleManager.CreateAsync(role);
                            await userManager.AddToRoleAsync(user, UserRoleName);
                        };
                    }

                    
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}

