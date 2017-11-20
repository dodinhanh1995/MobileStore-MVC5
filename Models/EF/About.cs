namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("About")]
    public partial class About
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage ="Tên không được hơn 50 ký tự.")]
        [Required(ErrorMessage ="Vui lòng nhập tên"), Display(Name ="Tên")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage ="Đường dẫn hình ảnh quá dài. Vui lòng nhập lại.")]
        [Display(Name ="Hình ảnh")]
        public string Image { get; set; }

        [Column(TypeName = "ntext"), Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Trạng thái"), Required(ErrorMessage ="Vui lòng chọn trạng thái.")]
        public bool Status { get; set; }
    }
}
