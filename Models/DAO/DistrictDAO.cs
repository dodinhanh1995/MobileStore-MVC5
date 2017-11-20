using Models.Extends;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace Models.DAO
{
    public class DistrictDAO
    {
        private static volatile DistrictDAO _instance;
        private static object key = new object();
        private string xmlPath = HttpContext.Current.Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml");

        public static DistrictDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (key)
                    {
                        _instance = new DistrictDAO();
                    }
                }
                return _instance;
            }
        }

        private DistrictDAO() { }

        public IEnumerable<DistrictModel> ListByProvinceId(int provinceId)
        {
            var xmlDoc = XDocument.Load(xmlPath);
            var xElements = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("id").Value == provinceId.ToString());

            var districtList = new List<DistrictModel>();

            foreach (var item in xElements.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                districtList.Add(new DistrictModel
                {
                    Id = int.Parse(item.Attribute("id").Value),
                    Name = item.Attribute("value").Value,
                    ProvinceId = provinceId
                });
            }


            return districtList;
        }

        public string GetNameById(int id)
        {
            var xmlDoc = XElement.Load(xmlPath);
            var element = xmlDoc.Elements("Item").Elements("Item")
                .Single(x => x.Attribute("type").Value == "district"
                && int.Parse(x.Attribute("id").Value) == id);

            return element.Attribute("value").Value;
        }
    }
}
