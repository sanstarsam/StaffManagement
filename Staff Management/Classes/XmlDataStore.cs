using System.Xml.Linq;

namespace Staff_Management.Classes
{
    public class XmlDataStore<T> where T : class, new()
    {
        private readonly string _filePath;
        private readonly string _rootName;

        public XmlDataStore(string filePath, string rootName = "Items")
        {
            _filePath = filePath;
            _rootName = rootName;

            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            if (!File.Exists(_filePath))
            {
                new XDocument(new XElement(_rootName)).Save(_filePath);
            }
        }

        public List<T> LoadAll(Func<XElement, T> map)
        {
            var doc = XDocument.Load(_filePath);
            return doc.Root?
                .Elements(typeof(T).Name)
                .Select(map)
                .ToList()
                ?? new List<T>();
        }

        public void SaveAll(IEnumerable<T> items, Func<T, XElement> map)
        {
            var doc = new XDocument(
                new XElement(_rootName,
                    items.Select(map)
                )
            );
            doc.Save(_filePath);
        }

        public void Append(T item, Func<T, XElement> map)
        {
            var doc = XDocument.Load(_filePath);
            doc.Root!.Add(map(item));
            doc.Save(_filePath);
        }
    }
}
