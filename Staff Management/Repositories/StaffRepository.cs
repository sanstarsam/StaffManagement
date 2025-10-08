using Staff_Management.Classes;
using Staff_Management.Models;
using System.Xml.Linq;

namespace Staff_Management.Repositories
{
    public interface IStaffRepository
    {
        List<StaffModel> GetAll();
        StaffModel? GetById(string staffId);
        void SaveAll(IEnumerable<StaffModel> list);
        void Add(StaffModel staff);
        bool Update(string staffId, StaffModel updated);
        bool Delete(string staffId);
    }
    public class StaffRepository : IStaffRepository
    {
        private readonly XmlDataStore<StaffModel> _store;

        public StaffRepository()
        {
            _store = new XmlDataStore<StaffModel>("Data/staffs.xml", "Staffs");
        }
        public StaffRepository(XmlDataStore<StaffModel> store)
        {
            _store = store;
        }

        private StaffModel MapToModel(XElement x) => new StaffModel
        {
            StaffId = (string)x.Element("StaffId"),
            FullName = (string)x.Element("FullName"),
            Birthday = DateOnly.Parse((string?)x.Element("Birthday") ?? "1900-01-01"),
            Gender = (int?)x.Element("Gender") ?? 0
        };

        private XElement MapToXml(StaffModel s) =>
            new XElement(nameof(StaffModel),
                new XElement("StaffId", s.StaffId),
                new XElement("FullName", s.FullName),
                new XElement("Birthday", s.Birthday.ToString("yyyy-MM-dd")),
                new XElement("Gender", s.Gender)
            );

        public List<StaffModel> GetAll() => _store.LoadAll(MapToModel);

        public StaffModel? GetById(string staffId)
        {
            var list = GetAll();
            return list.FirstOrDefault(x => x.StaffId.Equals(staffId, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveAll(IEnumerable<StaffModel> list) => _store.SaveAll(list, MapToXml);

        public void Add(StaffModel s) => _store.Append(s, MapToXml);

        public bool Update(string staffId, StaffModel updated)
        {
            var list = GetAll();
            var existing = list.FirstOrDefault(x => x.StaffId == staffId);
            if (existing == null) return false;

            existing.FullName = updated.FullName;
            existing.Birthday = updated.Birthday;
            existing.Gender = updated.Gender;

            SaveAll(list);
            return true;
        }

        public bool Delete(string staffId)
        {
            var list = GetAll();
            var target = list.FirstOrDefault(x => x.StaffId == staffId);
            if (target == null)
                return false;

            list.Remove(target);
            SaveAll(list);
            return true;
        }
    }
}
