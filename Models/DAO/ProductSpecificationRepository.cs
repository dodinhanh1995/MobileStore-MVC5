using Models.DAO.Repository;
using Models.EF;
using System.Data.SqlClient;

namespace Models.DAO
{
    public class ProductSpecificationRepository : GenericRepository<ProductSpecification>
    {
        public ProductSpecificationRepository(EntitiesDbContext db) : base(db) { }

        public async System.Threading.Tasks.Task<ProductSpecification> sp_SelectByIdAsync(int id)
        {
            return await _table.SqlQuery("sp_ProductSpecification @id", new SqlParameter("@id", id)).SingleOrDefaultAsync();
        }
    }
}
