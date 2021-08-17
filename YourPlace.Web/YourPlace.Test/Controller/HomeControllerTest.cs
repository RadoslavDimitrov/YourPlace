using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using Moq;

using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Test.Controller
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(c => c.Index())
            .ShouldReturn()
            .View();

        [Fact]
        public void IndexWithAuthorizedUsersShouldRedirect()
            => MyController<HomeController>
            .Instance(controller => controller.WithUser())
            .Calling(c => c.Index())
            .ShouldReturn()
            .Redirect(redirect => redirect
            .To<StoreController>(c => c.All(With.Any<AllStoresQueryModel>())));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(c => c.Error())
            .ShouldReturn()
            .View();

        
    }
}
