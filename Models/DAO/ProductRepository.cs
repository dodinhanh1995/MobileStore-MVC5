using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.DAO
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        public IPagedList<ProductViewModels> ListAllProductPaging(string sortOrder, string searchString, int? page)
        {
            var products = from p in _db.Database.SqlQuery<ProductViewModels>("sp_Product")
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                products = products.Where(x => x.Name.ToUpper().Contains(searchString) || x.Code.Contains(searchString) || x.CategoryName.ToUpper() == searchString);
            }

            products = sortOrder == "name" ? products.OrderBy(x => x.Name)
                : sortOrder == "name_desc" ? products.OrderByDescending(x => x.Name)
                : sortOrder == "vat" ? products.OrderBy(x => x.IncludeVAT)
                : sortOrder == "vat_desc" ? products.OrderByDescending(x => x.IncludeVAT)
                : sortOrder == "quantity" ? products.OrderBy(x => x.Quantity)
                : sortOrder == "quantity_desc" ? products.OrderByDescending(x => x.Quantity)
                : sortOrder == "category" ? products.OrderBy(x => x.CategoryName)
                : sortOrder == "category_desc" ? products.OrderByDescending(x => x.CategoryName)
                : sortOrder == "date" ? products.OrderBy(x => x.CreatedDate)
                : sortOrder == "date_desc" ? products.OrderByDescending(x => x.CreatedDate)
                : sortOrder == "topHot" ? products.Where(x => x.TopHot > DateTime.Now).OrderBy(x => x.TopHot)
                : sortOrder == "topHot_desc" ? products.Where(x => x.TopHot > DateTime.Now).OrderByDescending(x => x.TopHot)
                : sortOrder == "status" ? products.OrderBy(x => x.Status)
                : sortOrder == "status_desc" ? products.OrderByDescending(x => x.Status)
                : products.OrderByDescending(x => x.Id);

            return products.ToPagedList(page ?? 1, 7);
        }

        public IQueryable<ProductBriefViewModel> sp_ListProductByAction(string action)
        {
            var paramerters = new object[]
            {
                new SqlParameter("@action", action)
             };
            return _db.Database.SqlQuery<ProductBriefViewModel>("sp_Product @action", paramerters).AsQueryable();
        }

        public async Task<IEnumerable<ProductBriefViewModel>> SelectProductByFilterHasPagingAsync(string metaTitle, string orderBy, int pageIndex, int pageSize)
        {
            var paramerters = new object[]
           {
                new SqlParameter("@metaTitle", metaTitle),
                new SqlParameter("@orderBy", orderBy),
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@pageSize", pageSize)
            };
            return await _db.Database.SqlQuery<ProductBriefViewModel>("sp_ListProductByFilter @metaTitle, @orderBy, @pageIndex, @pageSize", paramerters).ToListAsync();
        }

        public async Task<IEnumerable<ProductRelatedViewModel>> SelectProductRelatedAsync(int id, decimal price)
        {
            var paramerters = new object[]
            {
                new SqlParameter("@id", id),
                new SqlParameter("@price", price)
            };
            return await _db.Database.SqlQuery<ProductRelatedViewModel>("sp_ListProductRelated @id, @price", paramerters).ToListAsync();
        }

        public async Task<decimal> GetPriceOrPromotionPriceById(int id)
        {
            var paramerters = new object[]
            {
                new SqlParameter("@action", "GetPriceOrPromotionPriceById"),
                new SqlParameter("@id", id)
            };
            return await _db.Database.SqlQuery<decimal>("sp_Product @action, @id", paramerters).SingleAsync();
        }

        public async Task<IEnumerable<ProductSearchViewModel>> ListProductByKeywordAsync(string keyword)
        {
            keyword = keyword.Trim().ToUpper();
            if (string.IsNullOrEmpty(keyword))  
                return null;

            var paramerters = new object[]
            {
                new SqlParameter("@action", "ListProductByKeyword"),
                new SqlParameter("@id", 1),
                new SqlParameter("@name", keyword.ToUpper())
            };
            return await _db.Database.SqlQuery<ProductSearchViewModel>("sp_Product @action, @id, @name", paramerters).ToListAsync();
        }

        public async Task<IEnumerable<ProductSearchViewModel>> ListNameByKeywordAsync(string keyword)
        {
            keyword = keyword.Trim().ToUpper();
            if (string.IsNullOrEmpty(keyword))
                return null;

            var paramerters = new object[]
            {
                new SqlParameter("@action", "ListNameByKeyword"),
                new SqlParameter("@id", 1),
                new SqlParameter("@name", keyword.ToUpper())
            };
            return await _db.Database.SqlQuery<ProductSearchViewModel>("sp_Product @action, @id, @name", paramerters).ToListAsync();
        }

        public string SelectNameById(int id)
        {
            return SelectProductDetailById(id).Name;
        }

        public string SelectMoreImagesById(int id)
        {
            return SelectProductDetailById(id).MoreImages;
        }

        public ProductDetailViewModel SelectProductDetailById(int id)
        {
            var paramerters = new object[]
            {
                new SqlParameter("@action", "GetDetailProductById"),
                new SqlParameter("@id", id)
             };
            return _db.Database.SqlQuery<ProductDetailViewModel>("sp_Product @action, @id", paramerters).SingleOrDefault();
        }

        public async Task<ProductDetailViewModel> SelectProductDetailByIdAsync(int id)
        {
            var paramerters = new object[]
            {
                new SqlParameter("@action", "GetDetailProductById"),
                new SqlParameter("@id", id)
             };
            return await _db.Database.SqlQuery<ProductDetailViewModel>("sp_Product @action, @id", paramerters).SingleOrDefaultAsync();
        }

        public bool sp_InsertByStoreProcedure(Product product)
        {
            product.PromotionPrice = product.PromotionPrice ?? 0;
            product.TopHot = product.TopHot ?? DateTime.Now;
            try
            {
                string query = "EXEC sp_Product @action, @id, @name, @code, @description, @image, @price, @promotionPrice, @includeVAT, @quantity, @categoryID, @createdBy,@metaTitle, @status, @topHot, @detail";
                object[] parameters = new object[]
                {
                    new SqlParameter("@action", "Insert"),
                    new SqlParameter("@id", product.Id),
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@code", product.Code),
                    new SqlParameter("@description", product.Description),
                    new SqlParameter("@image", product.Image),
                    new SqlParameter("@price", product.Price),
                    new SqlParameter("@promotionPrice", product.PromotionPrice),
                    new SqlParameter("@includeVAT", product.IncludeVAT),
                    new SqlParameter("@quantity", product.Quantity),
                    new SqlParameter("@categoryID", product.CategoryID),
                    new SqlParameter("@createdBy", product.CreatedBy),
                    new SqlParameter("@metaTitle", product.MetaTitle),
                    new SqlParameter("@status", product.Status),
                    new SqlParameter("@topHot", product.TopHot),
                    new SqlParameter("@detail", product.Detail)
                };
                return _db.Database.ExecuteSqlCommand(query, parameters) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool sp_UpdateByStoreProcedure(Product product)
        {
            product.PromotionPrice = product.PromotionPrice ?? 0;
            product.TopHot = product.TopHot ?? DateTime.Now;
            try
            {
                string query = "EXEC sp_Product @action, @id, @name, @code, @description, @image, @price, @promotionPrice, @includeVAT, @quantity, @categoryID, @createdBy,@metaTitle, @status, @topHot, @detail";
                object[] parameters = new object[]
                {
                    new SqlParameter("@action", "Update"),
                    new SqlParameter("@id", product.Id),
                    new SqlParameter("@name", product.Name),
                    new SqlParameter("@code", product.Code),
                    new SqlParameter("@description", product.Description),
                    new SqlParameter("@image", product.Image),
                    new SqlParameter("@price", product.Price),
                    new SqlParameter("@promotionPrice", product.PromotionPrice),
                    new SqlParameter("@includeVAT", product.IncludeVAT),
                    new SqlParameter("@quantity", product.Quantity),
                    new SqlParameter("@categoryID", product.CategoryID),
                    new SqlParameter("@createdBy", product.CreatedBy),
                    new SqlParameter("@metaTitle", product.MetaTitle),
                    new SqlParameter("@status", product.Status),
                    new SqlParameter("@topHot", product.TopHot),
                    new SqlParameter("@detail", product.Detail)
                };
                return _db.Database.ExecuteSqlCommand(query, parameters) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMoreImages(int id, string images)
        {
            try
            {
                var product = SelectById(id);
                product.MoreImages = images;
                Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<string> ConvertListImages(string moreImages)
        {
            try
            {
                XElement xImages = XElement.Parse(moreImages);
                List<string> imgList = new List<string>();

                foreach (XElement element in xImages.Elements())
                {
                    imgList.Add(element.Value);
                }
                return imgList;
            }
            catch
            {
                return null;
            }
        }

        public bool CheckNameIsExists(string name)
        {
            return _table.Any(m => m.Name.ToUpper() == name.Trim().ToUpper());
        }

        public string ChangingImage(int id, string image)
        {
            try
            {
                var product = this.SelectById(id);
                product.Image = image;
                Save();
                return string.Empty;
            }
            catch
            {
                return "Thông tin sản phẩm không tồn tại!";
            }
        }

        public async Task IncrementViewCountAsync(int id)
        {
            try
            {
                object[] parameters = new object[]
                {
                    new SqlParameter("@action", "IncrementViewCount"),
                    new SqlParameter("@id", id),
                };
                await _db.Database.ExecuteSqlCommandAsync("sp_Product @action, @id", parameters);
            }
            catch
            {

            }

        }

        public List<string> CheckQuantityIsEnough(int orderId)
        {
            return (from p in _table
                    join od in _db.OrderDetails
                    on p.Id equals od.ProductId
                    where p.Quantity - od.Quantity < 0 && od.OrderId == orderId
                    select p.Name).ToList();
        }

        public bool SubtractQuantityForOrder(int productId, int quantity)
        {
            try
            {
                SelectById(productId).Quantity -= quantity;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckCurrentQuantity(int id, int itemCount)
        {
            var product = await SelectProductDetailByIdAsync(id);
            return product.Quantity <= itemCount;
        }
    }
}
