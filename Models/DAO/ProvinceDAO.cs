using Models.Extends;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace Models.DAO
{
    public class ProvinceDAO
    {
        private static volatile ProvinceDAO _instance;
        private static object key = new object();
        private string xmlPath = HttpContext.Current.Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml");

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

        public IEnumerable<ProvinceModel> ListAll()
        {
            var xmlDoc = XDocument.Load(xmlPath);
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");

            var provinceList = new List<ProvinceModel>();

            foreach (var item in xElements)
            {
                provinceList.Add(new ProvinceModel
                {
                    Id = int.Parse(item.Attribute("id").Value),
                    Name = item.Attribute("value").Value
                });
            }

            return provinceList;
        }

        public string GetNameById(int id)
        {
            var xmlDoc = XElement.Load(xmlPath);
            var element = xmlDoc.Elements("Item")
                .Single(x => x.Attribute("type").Value == "province"
                && int.Parse(x.Attribute("id").Value) == id);

            return element.Attribute("value").Value;
        }
    }
}
