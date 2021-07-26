using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.User;

namespace YourPlace.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext data;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.data = data;
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = new User
            {
                Email = model.Email,
                UserName = model.Name
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return this.View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserFormModel model)
        {

            var user = await userManager.FindByNameAsync(model.Name);

            if (user == null)
            {
                return this.InvalidUsernameOrPassword(model);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordIsValid)
            {
                return this.InvalidUsernameOrPassword(model);
            }

            await this.signInManager.SignInAsync(user, true);

            return this.RedirectToAction("All", "Store");
        }

        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        private IActionResult InvalidUsernameOrPassword(LoginUserFormModel model)
        {
            const string InvalidUsernameOrPassword = "Wrong username or password!";

            ModelState.AddModelError(string.Empty, InvalidUsernameOrPassword);

            return this.View(model);
        }

        public IActionResult MyBookedHours()
        {
            var userId = this.User.GetId();

            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            var bookHour = this.data.BookedHours
                .Where(b => b.UserId == userId)
                .Select(b => new UserBookHourViewModel
                {
                    Id = b.Id,
                    StartFrom = b.StartFrom,
                    StoreName = b.StoreName,
                    StoreServiceId = b.StoreServiceId,
                    StoreServiceName = b.StoreServiceName
                })
                .ToList();
                

            //var bookedHours = user.bookedHours.Select(bh => new UserBookHourViewModel
            //{
            //    Id = bh.Id,
            //    StartFrom = bh.StartFrom,
            //    StoreName = bh.StoreName,
            //    StoreServiceId = bh.StoreServiceId,
            //    StoreServiceName = bh.StoreServiceName
            //})
            //.ToList();

            return this.View(bookHour);
        }
    }
}
