using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using YourPlace.Web.Models.Comment;

namespace YourPlace.Test.Route
{
    public class CommentControllerTest
    {
        [Fact]
        public void GetCreateShoudlBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Comment/Create")
            .To<CommentController>(c => c.Create(With.Any<string>()));

        [Fact]
        public void PostCreateShoudlBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/Comment/Create"))
            .To<CommentController>(c => c.Create(With.Any<string>()));

        [Fact]
        public void MineShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Comment/Mine")
            .To<CommentController>(c => c.Mine());

        [Fact]
        public void DeleteShouldBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Comment/Delete")
            .To<CommentController>(c => c.Delete(With.Any<string>()));
    }
}
