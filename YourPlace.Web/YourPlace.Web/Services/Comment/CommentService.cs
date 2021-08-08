
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using YourPlace.Data.Data;
using YourPlace.Web.Models.Comment;

namespace YourPlace.Web.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext data;
        private readonly IConfigurationProvider mapper;

        public CommentService(ApplicationDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public List<MineCommentsViewModel> CommentsByUser(string userId)
        {
            var comments = this.data.Comments
                .Where(c => c.UserId == userId)
                .ProjectTo<MineCommentsViewModel>(this.mapper)
                .ToList();

            return comments;
        }

        public string Create(string description, string storeId, string userId)
        {
            YourPlace.Models.Models.Comment comment = new YourPlace.Models.Models.Comment
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                StoreId = storeId,
                UserId = userId
            };

            this.data.Comments.Add(comment);
            this.data.SaveChanges();

            return comment.Id;
        }

        public bool Delete(string id, string userId)
        {
            var comment = this.data.Comments.Where(c => c.Id == id && c.UserId == userId).FirstOrDefault();

            if (comment == null)
            {
                return false;
            }

            var store = this.data.Stores.Where(s => s.Id == comment.StoreId).FirstOrDefault();

            if (store == null)
            {
                return false;
            }

            store.Comments.Remove(comment);
            this.data.Comments.Remove(comment);
            this.data.SaveChanges();

            return true;
        }
    }
}
