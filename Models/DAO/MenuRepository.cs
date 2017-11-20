using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models.DAO
{
    public class MenuRepository : GenericRepository<Menu>
    {
        public MenuRepository(EntitiesDbContext db) : base(db) { }

        public IEnumerable<Menu> SelectMenuByTypeId(int typeId)
        {
            return Select(m => m.TypeID == typeId);
        }

        public IEnumerable<NavigationViewModel> sp_SelectMenyByTypeId(int typeId)
        {
            return _db.Database.SqlQuery<NavigationViewModel>("sp_GetMenuByTypeId @typeId", new SqlParameter("@typeId", typeId));
        }

        public bool CheckNameIsExists(string text, int typeId)
        {
            return _table.Any(m => m.Text.ToUpper() == text.ToUpper() && m.TypeID == typeId);
        }
    }
}
