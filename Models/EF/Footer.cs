namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Footer")]
    public partial class Footer
    {
        [StringLength(10)]
        public string Id { get; set; }

        [Column(TypeName = "ntext"), Display(Name ="Nội dung"), Required(ErrorMessage ="Vui lòng nhập nội dung.")]
        public string Content { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn trạng thái.")]
        public bool Status { get; set; }
    }
}
