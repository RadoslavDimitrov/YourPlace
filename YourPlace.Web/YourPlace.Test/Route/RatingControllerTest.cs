using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using YourPlace.Web.Models.Rate;

namespace YourPlace.Test.Route
{
    public class RatingControllerTest
    {
        [Fact]
        public void GiveShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Rating/Give")
            .To<RatingController>(c => c.Give(With.Any<string>()));

        [Fact]
        public void PostGiveShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/Rating/Give"))
            .To<RatingController>(c => c.Give(With.Any<string>()));
    }
}
