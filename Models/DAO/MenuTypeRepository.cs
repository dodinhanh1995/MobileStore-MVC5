using Models.DAO.Repository;
using Models.EF;
using System.Linq;

namespace Models.DAO
{
    public class MenuTypeRepository : GenericRepository<MenuType>
    {
        public MenuTypeRepository(EntitiesDbContext db) : base(db) { }

        public bool CheckNameIsExists(string text)
        {
            return _table.Any(mt => mt.Name.ToUpper() == text.Trim().ToUpper());
        }
    }
}
