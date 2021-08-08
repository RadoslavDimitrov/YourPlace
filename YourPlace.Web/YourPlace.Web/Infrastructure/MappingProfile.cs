using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPlace.Models.Models;
using YourPlace.Web.Models.Comment;
using YourPlace.Web.Models.Store;
using YourPlace.Web.Models.StoreService;
using YourPlace.Web.Models.User;

namespace YourPlace.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            this.CreateMap<BookedHour, UserBookHourViewModel>();

            //StoreService
            this.CreateMap<StoreServices, DetailsStoreServiceViewModel>();

            //Store
            this.CreateMap<BookedHour, ListStoreBookedHoursViewModel>()
                .ForMember(u => u.Username, cfg => cfg.MapFrom(u => u.User.UserName));

            this.CreateMap<Store, ListStoreViewModel>()
                .ForMember(s => s.Town, cfg => cfg.MapFrom(s => s.Town.Name))
                .ForMember(s => s.District, cfg => cfg.MapFrom(s => s.District.Name))
                .ForMember(s => s.Raiting, cfg => cfg.MapFrom(s => s.Raitings.Select(r => r.StoreRaiting).Average()));

            //Comment
            this.CreateMap<Comment, MineCommentsViewModel>()
                .ForMember(c => c.StoreName, cfg => cfg.MapFrom(c => c.Store.Name));
        }
    }
}
