using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Models.DAO
{
    public class RoleDAO
    {
        private ApplicationDbContext _db = null;
        private static volatile RoleDAO instance = null;
        private UserManager<ApplicationUser> _user;
        private RoleManager<IdentityRole> _role;
        private static object key = new object();

        private RoleDAO()
        {
            _db = new ApplicationDbContext();
            _user = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            _role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
        }

        public static RoleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (key)
                    {
                        instance = new RoleDAO();
                    }
                }
                return instance;
            }
        }

        public IEnumerable<IdentityRole> ListAll()
        {
            return _db.Roles.ToList();
        }

        public IdentityRole SelectById(string id)
        {
            return _role.FindById(id);
        }

        public bool Insert(IdentityRole role)
        {
            try
            {
                _role.Create(role);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(IdentityRole role)
        {
            try
            {
                _role.Update(role);
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
                _role.Delete(SelectById(id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetRoleIdByUserId(string userId)
        {
            return _user.FindById(userId).Roles.FirstOrDefault().RoleId;
        }

        public string GetNameByRoleId(string roleId)
        {
            return _role.FindById(roleId).Name;
        }

        public bool CheckNameIsExists(string name)
        {
            return _role.FindByName(name.Trim()) != null;
        }
    }
}
