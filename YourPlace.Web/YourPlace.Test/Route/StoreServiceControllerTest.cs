using MyTested.AspNetCore.Mvc;
using YourPlace.Web.Controllers;
using Xunit;
using System.Collections.Generic;
using YourPlace.Web.Models.StoreService;
using System;

namespace YourPlace.Test.Route
{
    public class StoreServiceControllerTest
    {
        [Fact]
        public void GetCreateShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/StoreService/Create")
            .To<StoreServiceController>(c => c.Create(With.Any<string>()));

        [Fact]
        public void PostCreateShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/StoreService/Create/")
            .WithMethod(HttpMethod.Post))
            .To<StoreServiceController>(c => c.Create(With.Any<string>()));

        [Fact]
        public void DetailsShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/StoreService/Details")
            .To<StoreServiceController>(c => c.Details(With.Any<string>()));

        [Fact]
        public void GetEditShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/StoreService/Edit")
           .To<StoreServiceController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void PostEditShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap(request => request
           .WithPath("/StoreService/Edit")
           .WithMethod(HttpMethod.Post))
           .To<StoreServiceController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void DeleteShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/StoreService/Delete")
           .To<StoreServiceController>(c => c.Delete(With.Any<string>()));

        [Fact]
        public void BookAnHourShoudBeMapped()
           => MyRouting
           .Configuration()
           .ShouldMap("/StoreService/BookAnHour")
           .To<StoreServiceController>(c => c.BookAnHour(With.Any<string>(), With.Any<string>()));

        [Fact]
        public void CreateAnHourShoudBeMapped()
            => MyRouting
            .Configuration()
            .ShouldMap("/StoreService/CreateAnHour")
            .To<StoreServiceController>(c => c.CreateAnHour(
                With.Any<int>(),
                With.Any<string>(),
                With.Any<string>(),
                With.Any<string>(),
                With.Any<string>(),
                With.Any<string>()));
    }
}
