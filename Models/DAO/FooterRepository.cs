using Models.DAO.Repository;
using Models.EF;
using System.Linq;

namespace Models.DAO
{
    public class FooterRepository : GenericRepository<Footer>
    {
        public FooterRepository(EntitiesDbContext db) : base(db) { }

        public string GetFooter()
        {
            return _db.Database.SqlQuery<string>("sp_Footer").Single();
        }
    }
}
