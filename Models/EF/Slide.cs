namespace Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Slide")]
    public partial class Slide
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage ="Tên tối đa là 100 ký tự."), Display(Name="Tên")]
        [Required(ErrorMessage ="Vui lòng nhập tên quảng cáo.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn hình ảnh"), Display(Name="Hình ảnh")]
        [StringLength(200, ErrorMessage ="Đường dẫn hình ảnh tối đa 200 là ký tự.")]
        public string Image { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn thứ tự hiển thị."), Display(Name="Thứ tự")]
        [Range(0, 10, ErrorMessage ="Thứ tự phải từ 0 đến 10.")]
        public int DisplayOrder { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        [StringLength(20)]
        public string Target { get; set; }

        [Column(TypeName = "date"), DisplayFormat(DataFormatString ="{0:d}", ApplyFormatInEditMode =true)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn trạng thái."), Display(Name="Trạng thái")]
        public bool Status { get; set; }

        [StringLength(20), Display(Name="Vị trí")]
        public string Position { get; set; }
    }
}
