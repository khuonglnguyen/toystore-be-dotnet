using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToyStore.Data;
using ToyStore.Models;

namespace ToyStore.Service
{
    public interface ICommentService
    {
        Comment AddComment(Comment comment);
        IEnumerable<Comment> GetCommentByProductID(int ID);
    }
    public class CommentService : ICommentService
    {
        private readonly UnitOfWork context;
        public CommentService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Comment AddComment(Comment comment)
        {
            this.context.CommentRepository.Insert(comment);
            return comment;
        }
        public IEnumerable<Comment> GetCommentByProductID(int ID)
        {
            IEnumerable<Comment> listComment = this.context.CommentRepository.GetAllData(x=>x.ProductID==ID);
            return listComment;
        }
    }
}