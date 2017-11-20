using System;
using Common;
using System.Collections.Generic;

namespace Models.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ProductId { get; set; }

        private string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = Convertion.Truncate(value, 150);
            }
        }

        public DateTime CreatedDate { get; set; }
        public string DateCustom { get { return Convertion.ConvertDateTimeToString(CreatedDate); } }

        public bool Status { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public string MetaTitle { get; set; }
    }

    public class FullCommentViewModel
    {
        public CommentViewModel Comment { get; set; }
        public string Respondent { get; set; }
    }

    public class LoadProductCommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CommentDate { get { return Convertion.ConvertDateTimeToString(CreatedDate); } }
        public string Commenter { get; set; }
        public string RepresentCommenter { get { return Convertion.WordRepresentByText(Commenter); } }
        public IEnumerable<LoadProductCommentReplyViewModel> ReplyList { get; set; }
    }

    public class LoadProductCommentReplyViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ReplyDate { get { return Convertion.ConvertDateTimeToString(CreatedDate); } }
        public string Respondent { get; set; }
        public string RepresentRespondent { get { return Convertion.WordRepresentByText(Respondent); } }
        public string RoleName { get; set; }
    }

    public class ProductCommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
    }

}
