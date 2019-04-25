using System.Collections.Generic;
using Talking.Api.Data;

namespace Talking.Api.Repository
{
    public interface ICommentRepository
    {
        void InsertComment(Comment comment);
        IList<Comment> GetComments();
    }
}

