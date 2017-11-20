using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models.DAO
{
    //public class CommentReplyRepository : GenericRepository<CommentReply>
    //{
    //    //public CommentReplyRepository(EntitiesDbContext db) : base(db) { }

    //    //public IEnumerable<LoadProductCommentReplyViewModel> sp_SelectByCommentId(int commentId)
    //    //{
    //    //    var paramerters = new object[]
    //    //    {
    //    //                new SqlParameter("@action", "SelectByCommentId"),
    //    //                new SqlParameter("@id", 1),
    //    //                new SqlParameter("@commentId", commentId),
    //    //    };
    //    //    return _db.Database.SqlQuery<LoadProductCommentReplyViewModel>("EXEC sp_CommentReply @action, @id, @commentId", paramerters).ToList();
    //    //}

    //    //public bool sp_Insert(CommentReply reply)
    //    //{
    //    //    try
    //    //    {
    //    //        var paramerters = new object[]
    //    //        {
    //    //                new SqlParameter("@action", "Insert"),
    //    //                new SqlParameter("@id", 1),
    //    //                new SqlParameter("@commentId", reply.CommentId),
    //    //                new SqlParameter("@userId", reply.UserId),
    //    //                new SqlParameter("@content", reply.Content)
    //    //        };
    //    //        _db.Database.ExecuteSqlCommand("EXEC sp_CommentReply @action, @id, @commentId, @userId, @content", paramerters);
    //    //        return true;
    //    //    }
    //    //    catch
    //    //    {
    //    //        return false;
    //    //    }
    //    //}
    //}
}
