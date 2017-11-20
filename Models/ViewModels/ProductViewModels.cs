using Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ProductViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString ="{0:N0}đ", NullDisplayText = "")]
        public decimal? PromotionPrice { get; set; }

        public bool IncludeVAT { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", NullDisplayText = "")]
        public DateTime? TopHot { get; set; }
        public int ViewCount { get; set; }
    }

    public class ProductRelatedViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PromotionPrice { get; set; }

        public string Screen { get; set; }
        public string CameraAfter { get; set; }
        public string CameraBefore { get; set; }
        public string PinCapacity { get; set; }
    }

    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string MoreImages { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PromotionPrice { get; set; }

        public int Quantity { get; set; }
        public bool IncludeVAT { get; set; }
        public string CategoryName { get; set; }
        public string CategoryMetaTitle { get; set; }
    }

    public class ProductBriefViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal Price { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}đ")]
        public decimal? PromotionPrice { get; set; }

        public int Quantity { get; set; }
        public string MetaTitle { get; set; }
        public int ViewCount { get; set; }
    }

    public class ProductSearchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public string MetaTitle { get; set; }
    }

    public class DetailPageViewModel
    {
        public ProductDetailViewModel Product { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
        public IEnumerable<ProductRelatedViewModel> ProductRelated { get; set; }

    }
}
