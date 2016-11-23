using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Risk.Model;

namespace Risk.Serialization
{
    public class Serializer
    {
        public static Serializer Instance { get; } = new Serializer();

        private Serializer() { }

        /* SERIALIZATION */
        public async void AsyncSerializeToFile(Map map, string path)
        {
            await Task.Run(() => SerializeToFile(map, path));
        }

        private void SerializeToFile(Map map, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var serializer = new DataContractSerializer(typeof(Map));
                serializer.WriteObject(stream, map);
            }
        }

        private string SerializeToString(Map diagram)
        {
            var stringBuilder = new StringBuilder();

            using (TextWriter stream = new StringWriter(stringBuilder))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Map));
                serializer.Serialize(stream, diagram);
            }

            return stringBuilder.ToString();
        }

        public Task<string> AsyncSerializeToString(Map map)
        {
            return Task.Run(() => SerializeToString(map));
        }

        /* DESERIALIZATION */

        public Task<Map> AsyncDeserializeFromFile(string path)
        {
            return Task.Run(() => DeserializeFromFile(path));
        }

        private Map DeserializeFromFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(Map));
                var diagram = serializer.ReadObject(reader, true) as Map;

                return diagram;
            }
        }

        public Task<Map> AsyncDeserializeFromString(string xml)
        {
            return Task.Run(() => DeserializeFromString(xml));
        }

        private Map DeserializeFromString(string xml)
        {
            using (TextReader stream = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Map));
                Map diagram = serializer.Deserialize(stream) as Map;

                return diagram;
            }
        }
    }

}
