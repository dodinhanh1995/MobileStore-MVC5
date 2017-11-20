using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class IndexViewModel
    {
        public string PhoneNumber { get; set; }
    }

    public class ChangePhoneNumberViewModel
    {
        [Display(Name = "Điện thoại"), Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression("^(084|\\+84|84|0)\\d{9,10}", ErrorMessage = "Số điện thoại không hợp lệ. Vui lòng thử  lại.")]
        public string PhoneNumber { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}