namespace Models.EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên danh mục sản phẩm."), Display(Name="Danh mục sản phẩm")]
        [StringLength(50, ErrorMessage = "Tên danh mục sản phẩm tối đa là 50 ký tự.")]
        public string Name { get; set; }

        [Display(Name="Thuộc về danh mục")]
        public int? ParentID { get; set; }

        [Display(Name="Thứ tự hiển thị"), Required(ErrorMessage ="Vui lòng chọn thứ tự hiển thị.")]
        [Range(0, 20, ErrorMessage ="Thứ tự phải từ 0 đến 20")]
        public int DisplayOrder { get; set; }

        [StringLength(50, ErrorMessage = "MetaTitle tối đa là 50 ký tự."), Required(ErrorMessage = "Vui lòng nhập tên MetaTitle.")]
        public string MetaTitle { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn trạng thái."),  Display(Name="Trạng thái")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
