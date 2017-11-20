namespace Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Contact")]
    public partial class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập họ tên."), Display(Name = "Họ tên")]
        [StringLength(50, ErrorMessage ="Họ tên không được hơn 50 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại."), Display(Name="Điện thoại")]
        [RegularExpression(@"^(\+084|\+84|84|084|0)\d{9,10}$", ErrorMessage ="Số điện thoại không hợp lệ.")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập địa chỉ email."), EmailAddress(ErrorMessage ="Địa chỉ email không hợp lệ.")]
        [StringLength(50, ErrorMessage ="Địa chỉ email không được hơn 50 ký tự.")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage ="Nội dung không được hơn 500 ký tự"), Required(ErrorMessage ="Vui lòng nhập nội dung.")]
        public string Content { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn trạng thái.")]
        public bool Status { get; set; }
    }
}
