namespace Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        public int RecordId { get; set; }

        [StringLength(100)]
        public string CartId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
