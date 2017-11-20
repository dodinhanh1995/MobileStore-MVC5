using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO.Repository
{
    public class ProvinceDAO
    {
        private static volatile ProvinceDAO _instance;
        private static object key = new object();

        public static ProvinceDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (key)
                    {
                        _instance = new ProvinceDAO();
                    }
                    
                }
                return _instance;
            }
        }

        private ProvinceDAO() { }
    }
}
