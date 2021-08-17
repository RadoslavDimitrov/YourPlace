using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

using YourPlace.Data.Data;
using YourPlace.Models.Models;
using YourPlace.Web.Infrastructure;
using YourPlace.Web.Models.User;
using YourPlace.Web.Services.User;
using static YourPlace.Web.Infrastructure.RoleConstants;

namespace YourPlace.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserService userService;

        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userService = userService;
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

            if(await roleManager.RoleExistsAsync(UserRoleName))
            {
                await userManager.AddToRoleAsync(user, UserRoleName);
            }
            else
            {
                var role = new IdentityRole { Name = UserRoleName };

                await roleManager.CreateAsync(role);
                await userManager.AddToRoleAsync(user, UserRoleName);
            };
            

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

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        

        [Authorize]
        public IActionResult MyBookedHours()
        {
            var userId = this.User.GetId();

            var bookedHours = this.userService.UserBookedHours(userId);
               
            return this.View(bookedHours);
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = this.userService.UserWithRole(this.User.GetId());

            if(user == null)
            {
                return View("NotFound", ApplicationMessages.Exception.UserDoesNotExist);
            }

            return this.View(user);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>  ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return this.View("NotFound", ApplicationMessages.Exception.UserDoesNotExist);
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return this.View();
            }

            await signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Profile", "User");
        }


        private IActionResult InvalidUsernameOrPassword(LoginUserFormModel model)
        {
            const string InvalidUsernameOrPassword = "Wrong username or password!";

            ModelState.AddModelError(string.Empty, InvalidUsernameOrPassword);

            return this.View(model);
        }
    }
}
