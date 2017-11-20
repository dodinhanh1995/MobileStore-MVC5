using Models.DAO.Repository;
using Models.EF;
using System.Collections.Generic;
using System.Linq;
using static Common.Enum.EDatabase;

namespace Models.DAO
{
    public class ContactRepository : GenericRepository<Contact>
    {
        public ContactRepository(EntitiesDbContext db) : base(db) { }

        private UnitOfWork _unitOW = new UnitOfWork();

        public IEnumerable<Contact> GetListByFilterAndSort(string sortOrder, string key, EContactColumnsName columnName)
        {
            var contacts = from c in this.Select()
                           select c;

            if (!string.IsNullOrEmpty(key))
            {
                key = key.ToUpper();
                contacts = columnName == EContactColumnsName.Name ? contacts.Where(c => c.Name.ToUpper().Contains(key))
                    : columnName == EContactColumnsName.Email ? contacts.Where(c => c.Email.ToUpper().Contains(key))
                    : columnName == EContactColumnsName.Phone ? contacts.Where(c => c.Phone.ToUpper().Contains(key))
                    : contacts.Where(c => c.Name.ToUpper().Contains(key)
                                                || c.Email.ToUpper().Contains(key)
                                                || c.Phone.ToUpper().Contains(key));
            }

            contacts = sortOrder == "name_desc" ? contacts.OrderByDescending(c => c.Name)
                    : sortOrder == "status_desc" ? contacts.OrderByDescending(c => c.Status)
                    : sortOrder == "status" ? contacts.OrderBy(c => c.Status)
                    : sortOrder == "date" ? contacts.OrderBy(c => c.CreatedDate)
                    : contacts.OrderBy(c => c.Name);

            return contacts.ToList();
        }

        public bool ChangingStatus(int id)
        {
            Contact contact = this.SelectById(id);
            contact.Status = !contact.Status;

            Update(contact);
            Save();
            
            return contact.Status;
        }

        public bool Create(Contact contact)
        {
            try
            {
                contact.CreatedDate = System.DateTime.Now;
                Insert(contact);
                Save();
                return true;
            }
            catch
            {
                return false;
            }
           
            
        }
    }
}
