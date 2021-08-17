
using Xunit;
using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;

namespace YourPlace.Test.Route
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index());

        [Fact]
        public void ErrorShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());
    }
}
