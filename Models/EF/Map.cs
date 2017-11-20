namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Map")]
    public partial class Map
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên."), Display(Name="Tên bản đồ")]
        [StringLength(50, ErrorMessage ="Tên không được hơn 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập APIKey.")]
        [StringLength(50, ErrorMessage = "APIKey không được hơn 50 ký tự.")]
        public string APIKey { get; set; }

        [StringLength(20, ErrorMessage ="Vĩ độ phải nhỏ hơn 20 ký tự"),  Required(ErrorMessage = "Vui lòng nhập vĩ độ.")]
        [Display(Name = "Vĩ độ")]
        public string Latitude { get; set; }

        [StringLength(20, ErrorMessage ="Kinh độ phải nhỏ hơn 20 ký tự"), Display(Name = "Kinh độ")]
        [Required(ErrorMessage ="Vui lòng nhập kinh độ.")]
        public string Longitude { get; set; }

        [StringLength(500, ErrorMessage ="Nội dung tối đa là 500 ký tự."), Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        [Display(Name = "Nội dung")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái"), Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
}
