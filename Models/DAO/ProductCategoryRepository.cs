using Models.DAO.Repository;
using Models.EF;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Models.ViewModels;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>
    {
        public ProductCategoryRepository(EntitiesDbContext db) : base(db) { }

        public IEnumerable<ProductCategory> GetDropDownLists(int? id = null)
        {
            var productCategories = (from pc in _table
                                     select new
                                     {
                                         Id = pc.Id,
                                         Name = pc.Name
                                     });
            if (id.HasValue)
                productCategories = productCategories.Where(pc => pc.Id != id.Value);

            return productCategories.ToList().Select(x => new ProductCategory { Id = x.Id, Name = x.Name });
        }

        public async Task<IEnumerable<CategoryNameViewModel>> sp_GetListCategoryNameAsync()
        {
            return await _db.Database.SqlQuery<CategoryNameViewModel>("sp_ProductCategory @action", new SqlParameter("@action", "ListCategoryName")).ToListAsync();
        }

        public bool CheckNameIsExists(string name)
        {
            return _table.Any(pc => pc.Name.ToUpper() == name.Trim().ToUpper());
        }
    }
}
