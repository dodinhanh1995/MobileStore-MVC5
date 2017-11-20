using Models.DAO.Repository;
using Models.EF;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Common.Enum.EDatabase;

namespace Models.DAO
{
    public class SlideRepository : GenericRepository<Slide>
    {
        public SlideRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        public IEnumerable<AdvertisementViewModel> GetSlideByPosition(ESlidePosition position)
        {
            var parameters = new object[]
            {
                new SqlParameter("@position", position.ToString())
            };
            return _db.Database.SqlQuery<AdvertisementViewModel>("sp_GetSlideByPosition @position", parameters).ToList();
        }

        public async Task<IEnumerable<AdvertisementViewModel>> GetSlideByPositionAsync(ESlidePosition position)
        {
            var parameters = new object[]
            {
                new SqlParameter("@position", position.ToString())
            };
            return await _db.Database.SqlQuery<AdvertisementViewModel>("sp_GetSlideByPosition @position", parameters).ToListAsync();
        }

        public bool CheckNameIsExists(string name)
        {
            return _table.Any(x => x.Name.ToUpper() == name.Trim().ToUpper());
        }

        public string ChangingImage(int id, string image)
        {

            try
            {
                var slide = SelectById(id);
                slide.Image = image;
                Save();
                return string.Empty;
            }
            catch (Exception)
            {
                return "Thông tin quảng cáo không tồn tại!";
            }
        }
    }
}
