namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        public string CustomerId { get; set; }
   
        [Required(ErrorMessage = "Vui lòng nhập họ và tên."), StringLength(50)]
        public string ShipName { get; set; }

        [Required(ErrorMessage = "Vui chọn giới tính.")]
        public bool ShipGender { get; set; }

        [Required(ErrorMessage = "Vui lòng số điện thoại."), RegularExpression("^(084|\\+84|84|0)\\d{9,10}", ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ.")]
        public string ShipMobile { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn tỉnh, thành")]
        public int ProvinceId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quận, huyện")]
        public int DistrictId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số nhà, tên đường, phường / xã."), StringLength(250)]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email."), EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
        public string ShipEmail { get; set; }

        public int Status { get; set; }

        public decimal? Total { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
