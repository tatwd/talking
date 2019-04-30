using System.Collections.Generic;
using Talking.Api.Data;

namespace Talking.Api.Repository
{
    public interface ICommentRepository
    {
        void InsertComment(Comment comment);
        IList<Comment> GetComments();
        IList<Comment> GetComments(string postUrl);
        IList<Comment> GetComments(string postUrl,  int page, int limit);
    }
}

