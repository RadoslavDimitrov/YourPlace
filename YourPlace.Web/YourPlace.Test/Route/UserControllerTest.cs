using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using YourPlace.Web.Models.User;

namespace YourPlace.Test.Route
{
    public class UserControllerTest
    {
        [Fact]
        public void GetRegisterShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/User/Register")
            .To<UserController>(c => c.Register());

        [Theory]
        [InlineData("Rado", "Test12", "Rado@rado.bg")]
        public void PostRegisterShoudBeMapped(string name, string password, string email)
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/User/Register")
            .WithFormFields(new RegisterUserFormModel 
            {
                Name = name,
                Email = email,
                Password = password
            }))
            .To<UserController>(c => c.Register(new RegisterUserFormModel 
            {
                Name = name,
                Email = email,
                Password = password
            }));


        [Fact]
        public void GetLoginShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/User/Login")
            .To<UserController>(c => c.Login());

        [Theory]
        [InlineData("Rado", "test12")]
        public void PostLoginShoudBeMapped(string name, string password)
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/User/Login")
            .WithFormFields(new LoginUserFormModel 
            {
                Name = name,
                Password = password
            }))
            .To<UserController>(c => c.Login(new LoginUserFormModel 
            {
                Name = name,
                Password = password
            }));

        [Fact]
        public void LogoutShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/User/Logout")
           .To<UserController>(c => c.Logout());

        [Fact]
        public void MyBookedHoursShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/User/MyBookedHours")
           .To<UserController>(c => c.MyBookedHours());

        [Fact]
        public void ProfileShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/User/Profile")
           .To<UserController>(c => c.Profile());

        [Fact]
        public void GetChangePasswordShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/User/ChangePassword")
           .To<UserController>(c => c.ChangePassword());

        [Theory]
        [InlineData("Test12", "Test123", "Test123")]
        public void PostChangePasswordShoudBeMapped(string oldPassword, string newPassword, string confirmPassword)
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/User/ChangePassword")
            .WithFormFields(new ChangePasswordViewModel 
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            }))
            .To<UserController>(c => c.ChangePassword(new ChangePasswordViewModel 
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                ConfirmPassword = confirmPassword
            }));
    }
}
