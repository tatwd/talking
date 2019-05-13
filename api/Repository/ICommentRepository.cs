using System.Collections.Generic;
using Talking.Api.Data;

namespace Talking.Api.Repository
{
    public interface ICommentRepository
    {
        void InsertComment(Comment comment);
        long GetCount(string postUrl);
        IList<Comment> GetComments(int limit = 0); // 0 is to get all
        IList<Comment> GetComments(string postUrl);
        IList<Comment> GetComments(string postUrl,  int page, int limit);
    }
}

