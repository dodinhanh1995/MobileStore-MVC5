using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using PagedList;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models.DAO
{
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        public IPagedList<FullCommentViewModel> ListAllCommentPaging(string sortOrder, string searchString, int? page)
        {
            var products = from p in ListComment()
                           select new FullCommentViewModel
                           {
                               Comment = p,
                               Respondent = GetReplyUserName(p.ParentId.GetValueOrDefault())
                           };

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                products = products.Where(x => x.Comment.UserName.ToUpper().Contains(searchString) 
                || x.Comment.Email.Contains(searchString) 
                || x.Comment.FullName.ToUpper().Contains(searchString)
                || x.Comment.ProductName.ToUpper().Contains(searchString)
                || x.Comment.Content.ToUpper().Contains(searchString));
            }

            products = sortOrder == "date" ? products.OrderBy(x => x.Comment.CreatedDate)
                : sortOrder == "date_desc" ? products.OrderByDescending(x => x.Comment.CreatedDate)
                : sortOrder == "status" ? products.OrderBy(x => x.Comment.Status)
                : sortOrder == "status_desc" ? products.OrderByDescending(x => x.Comment.Status)
                : products.OrderByDescending(x => x.Comment.CreatedDate);

            return products.ToPagedList(page ?? 1, 10);
        }

        public IEnumerable<CommentViewModel> ListComment()
        {
            return _db.Database.SqlQuery<CommentViewModel>("sp_Comment @action", new SqlParameter("@action", "List")).ToList();
        }

        public string GetReplyUserName(int parentId)
        {
            var parameters = new object[]
            {
                new SqlParameter("@action", "GetReplyUserName"),
                new SqlParameter("@id", parentId)
            };
            return _db.Database.SqlQuery<string>("sp_Comment @action, @id", parameters).SingleOrDefault();
        }

        public IEnumerable<LoadProductCommentViewModel> ListByProductId(int productId)
        {
            return (from c in sp_ListCommentByProductId(productId)
                    select new LoadProductCommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        Commenter = c.FullName,
                        CreatedDate = c.CreatedDate,
                        ReplyList = _unitOW._CommentRepo.sp_ListReplyByCommentId(c.Id)
                    }).ToList();
        }

        public IQueryable<ProductCommentViewModel> sp_ListCommentByProductId(int productId)
        {
            var paramerters = new object[]
                {
                        new SqlParameter("@action", "ListCommentByProductId"),
                        new SqlParameter("@id", 1),
                        new SqlParameter("@userId", 1),
                        new SqlParameter("@productId", productId),
                };
            return _db.Database.SqlQuery<ProductCommentViewModel>("sp_Comment @action, @id, @userId, @productId", paramerters).AsQueryable();
        }

        public IEnumerable<LoadProductCommentReplyViewModel> sp_ListReplyByCommentId(int id)
        {
            var paramerters = new object[]
            {
                        new SqlParameter("@action", "ListReplyById"),
                        new SqlParameter("@id", id),
            };
            return _db.Database.SqlQuery<LoadProductCommentReplyViewModel>("EXEC sp_Comment @action, @id", paramerters).ToList();
        }

        public int sp_InsertComment(Comment comment)
        {
            try
            {
                var paramerters = new object[]
                {
                        new SqlParameter("@action", "InsertComment"),
                        new SqlParameter("@id", comment.Id),
                        new SqlParameter("@userId", comment.UserId),
                        new SqlParameter("@productId", comment.ProductId),
                        new SqlParameter("@content", comment.Content)
                };
                return (int)_db.Database.SqlQuery<decimal>("sp_Comment @action, @id, @userId, @productId, @content", paramerters).SingleOrDefault();
            }
            catch
            {
                return -1;
            }
        }

        public bool sp_InsertReply(Comment reply)
        {
            try
            {
                var paramerters = new object[]
                {
                        new SqlParameter("@action", "InsertReply"),
                        new SqlParameter("@id", 1),
                        new SqlParameter("@userId", reply.UserId),
                        new SqlParameter("@productId", reply.ProductId),
                        new SqlParameter("@content", reply.Content),
                        new SqlParameter("@parentId", reply.ParentId)
                };
                _db.Database.ExecuteSqlCommand("EXEC sp_Comment @action, @id, @userId, @productId, @content, @parentId", paramerters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var comment = SelectById(id);
                comment.Status = !comment.Status;
                Save();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
