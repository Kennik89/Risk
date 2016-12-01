using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Risk.Model;

namespace Risk.LoadSave
{
    public class Serializer
    {
        public static Serializer Instance { get; } = new Serializer();

        [STAThread]
        public void Save()
        {
            // For debug
            var s1 = new Shape() { X = 123, Y = 69 };

            // TODO: Allow to serialize multiple objects at once
            var writer = new FileStream("Datafile.xml", FileMode.Create);
            var ser = new DataContractSerializer(typeof(Shape));
            ser.WriteObject(writer, s1);
            writer.Close();
        }

        public void Load()
        {
            var fs = new FileStream("Datafile.xml", FileMode.Open);
            var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(Shape));

            // Deserialize the data and read it from the instance.
            var deserializedObjects = (Shape)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            // TODO: Add the shape on the canvas
        }
    }
}
