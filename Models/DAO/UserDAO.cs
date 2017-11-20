using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class UserDAO
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _user;
        private RoleManager<IdentityRole> _role;
        private static volatile UserDAO instance = null;
        private static object key = new object();

        private UserDAO()
        {
            _db = new ApplicationDbContext();
            _user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            _role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
        }

        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (key)
                    {
                        instance = new UserDAO();
                    }
                }
                return instance;
            }
        }

        public IEnumerable<ListUserViewModel> ListAllAsync()
        {
            var user = from u in _user.Users
                       select new
                       {
                           Id = u.Id,
                           UserName = u.UserName,
                           FullName = u.FullName,
                           Email = u.Email,
                           PhoneNumber = u.PhoneNumber,
                       };

            return user.ToList().Select(x => new ListUserViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                RoleName = _user.GetRoles(x.Id).FirstOrDefault()
            });
        }

        public ApplicationUser SelectById(string id)
        {
            return _user.FindById(id);
        }

        public string GetIdByUserName(string userName)
        {
            return _user.FindByName(userName).Id;
        }

        public async Task<string> GetIdByUserNameAsync(string userName)
        {
            return await (from u in _db.Users
                          where u.UserName == userName
                          select u.Id).SingleOrDefaultAsync();
        }

        public bool Update(ApplicationUser user, string roleId)
        {
            try
            {
                var oldRoleName = RoleDAO.Instance.GetNameByRoleId(user.Roles.FirstOrDefault().RoleId);
                var newRoleName = RoleDAO.Instance.SelectById(roleId).Name;
            
                if (oldRoleName != null)
                {
                    _user.RemoveFromRoles(user.Id, oldRoleName);
                    _user.AddToRole(user.Id, newRoleName);
                }
                _user.Update(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                _user.Delete(SelectById(id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckUsernameIsExists(string username)
        {
            return _user.FindByName(username) != null;
        }

        public bool CheckEmailIsExists(string email)
        {
            return _user.FindByEmail(email) != null;
        }
    }
}
