namespace Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? Status { get; set; }

        public virtual Product Product { get; set; }
    }
}
