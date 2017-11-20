namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm.")]
        [StringLength(100, ErrorMessage ="Tên sản phẩm tối đa là 100 ký tự."), Display(Name="Tên")]
        public string Name { get; set; }

        [StringLength(10, ErrorMessage ="Code tối đa là 10 ký tự."), Required(ErrorMessage ="Vui lòng nhập code sản phẩm.")]
        public string Code { get; set; }

        [Column(TypeName ="ntext"), Display(Name = "Chi tiết sản phẩm")]
        public string Detail { get; set; }

        [Column(TypeName = "ntext"), Display(Name="Mô tả sản phẩm")]
        public string Description { get; set; }

        [StringLength(250, ErrorMessage ="Đường dẫn hình ảnh tối đa là 250 ký tự.")]
        [Required(ErrorMessage ="Vui lòng chọn hình ảnh."), Display(Name="Hình ảnh")]
        public string Image { get; set; }

        [Column(TypeName = "xml"), Display(Name="Danh sách hình ảnh")]
        public string MoreImages { get; set; }

        [DataType(DataType.Currency, ErrorMessage ="Giá sản phẩm không hợp lệ."), DisplayFormat(DataFormatString ="{0:c}")]
        [Required(ErrorMessage ="Vui lòng nhập giá sản phẩm.")]
        public decimal Price { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Giá khuyến mại không hợp lệ."), DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name="Giá khuyến mại")]
        public decimal? PromotionPrice { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn VAT."), Display(Name="VAT")]
        public bool IncludeVAT { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập số lượng."), Range(0, 100, ErrorMessage ="Số lượng phải từ 0 đến 100.")]
        [Display(Name="Số lượng")]
        public int Quantity { get; set; }

        [Display(Name="Danh mục"), Required(ErrorMessage ="Vui lòng chọn danh mục.")]
        public int CategoryID { get; set; }

        [Column(TypeName = "date"), Display(Name="Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [Display(Name="Người tạo"), StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(100, ErrorMessage ="MetaTitle tối đa là 100 ký tự."), Required(ErrorMessage ="Vui lòng nhập MetaTitle")]
        public string MetaTitle { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn trạng thái"), Display(Name="Trạng thái")]
        public bool Status { get; set; }

        [Display(Name="Hiển thị tại trang chủ"), DisplayFormat(DataFormatString ="{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? TopHot { get; set; }

        public int ViewCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ProductSpecification ProductSpecification { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
