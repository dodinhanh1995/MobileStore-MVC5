namespace Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductSpecification")]
    public partial class ProductSpecification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Screen { get; set; }

        [StringLength(100)]
        public string OperatingSystem { get; set; }

        [StringLength(100)]
        public string CameraAfter { get; set; }

        [StringLength(100)]
        public string CameraBefore { get; set; }

        [StringLength(50)]
        public string CPU { get; set; }

        [StringLength(20)]
        public string RAM { get; set; }

        [StringLength(30)]
        public string InternalMemory { get; set; }

        [StringLength(50)]
        public string SIM { get; set; }

        [StringLength(50)]
        public string PinCapacity { get; set; }

        public virtual Product Product { get; set; }
    }
}
