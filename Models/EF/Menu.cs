namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Menu")]
    public partial class Menu
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên menu."), Display(Name ="Tên menu")]
        [StringLength(50, ErrorMessage ="Tên menu tối đa là 50 ký tự.")]
        public string Text { get; set; }

        [StringLength(100, ErrorMessage = "Icon tối đa là 100 ký tự.")]
        public string Icon { get; set; }

        [StringLength(250, ErrorMessage = "Link tối đa là 250 ký tự.")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thứ tự hiển thị."), Display(Name ="Thứ tự hiển thị")]
        [Range(0,20, ErrorMessage ="Thứ tự phải từ 0 đến 20")]
        public int DisplayOrder { get; set; }

        [StringLength(10)]
        public string Target { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái."), Display(Name ="Trạng thái")]
        public bool Status { get; set; }

        [Display(Name ="Loại menu")]
        public int? TypeID { get; set; }

        public virtual MenuType MenuType { get; set; }
    }
}
