using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;

using YourPlace.Test.Data;

namespace YourPlace.Test.Controller
{
    public class CommentControllerTest
    {

        [Fact]
        public void CreateShouldReturnView()
            => MyController<CommentController>
            .Instance()
            .Calling(c => c.Create(GetData.GetId))
            .ShouldReturn()
            .View();
            
    }
}
