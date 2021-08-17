using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Areas.Admin.Controllers;
using Xunit;
using System.Collections.Generic;
using System;
using YourPlace.Web.Areas.Admin.Models;

namespace YourPlace.Test.Route
{
    public class AdminControllerTest
    {
        [Fact]
        public void AllUsersShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Admin/AllUsers")
            .To<AdminController>(c => c.AllUsers());

        [Fact]
        public void DeleteUserShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Admin/DeleteUser")
            .To<AdminController>(c => c.DeleteUser(With.Any<string>()));

        [Fact]
        public void GetChangeRoleShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Admin/Admin/ChangeRole")
            .To<AdminController>(c => c.ChangeRole(With.Any<string>()));

        [Theory]
        [InlineData("12c6fa2d-1c30-40e6-aa25-a4ef7a7bf426", "User")]
        public void GetChangeRoleShouldBeMapped(string id, string roleName)
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/Admin/Admin/ChangeRole")
            .WithFormFields(new ChangeUserRoleViewModel
            {
                Id = id,
                Rolename = roleName
            }))
            .To<AdminController>(c => c.ChangeRole(new ChangeUserRoleViewModel
            {
                Id = id,
                Rolename = roleName
            },
                With.Any<string>()));
            
    }
}
