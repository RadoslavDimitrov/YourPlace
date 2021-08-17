using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Services.Store.Models;

namespace YourPlace.Test.Route
{
    public class StoreControllerTest
    {
        [Fact]
        public void GetCreateShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Store/Create")
            .To<StoreController>(c => c.Create());

        [Fact]
        public void PostCreateShoudBeMapped()
             => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithMethod(HttpMethod.Post)
            .WithPath("/Store/Create"))
            .To<StoreController>(c => c.Create(With.Any<CreateStoreViewModel>()));

        [Fact]
        public void MyStoreShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Store/MyStore")
            .To<StoreController>(c => c.MyStore());

        [Fact]
        public void AllShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Store/All")
            .To<StoreController>(c => c.All(With.Any<AllStoresQueryModel>()));

        [Fact]
        public void VisitShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/Store/Visit")
            .To<StoreController>(c => c.Visit(With.Any<string>()));

        [Fact]
        public void MyStoreBookedHoursShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/Store/MyStoreBookedHours")
           .To<StoreController>(c => c.MyStoreBookedHours());
    }
}
