using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;

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
    }
}
