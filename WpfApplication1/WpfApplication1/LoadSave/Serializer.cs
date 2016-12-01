using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Risk.Model;
using System.Collections.ObjectModel;

namespace Risk.LoadSave
{
    public class Serializer
    {
        public static Serializer Instance { get; } = new Serializer();

        [STAThread]
        public void Save(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines)
        {
            //https://msdn.microsoft.com/en-us/library/ms752244(v=vs.110).aspx

            FileStream writer = new FileStream("C:/Users/Martin/Datafile.xml", FileMode.Create);
            
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Map));

            //Create a Map object with lines and shapes
            Map m = new Map();

            //Insert shapes to map
            if (shapes.Count > 0)
            {

                foreach (Shape shape in shapes)
                {
                    m.countries.Add(shape);
                }
                //Console.Write("Teest");

            }

            //Insert lines to map
            if (lines.Count > 0)
            {
                foreach (Line line in lines)
                {
                    m.connections.Add(new serialLine(line));
                }
            }

            //Serialize the map
            serializer.WriteObject(writer, m);

            writer.Close();
        }

        public void Load(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines)
        {
            //http://stackoverflow.com/questions/16943176/how-to-deserialize-xml-using-datacontractserializer
            //Define a map
            Map m;
            //Read from file
            DataContractSerializer dcs = new DataContractSerializer(typeof(Map));
            FileStream fs = new FileStream("C:/Users/Martin/Datafile.xml", FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            
            //Actual reading and assigning to map
            m = (Map) dcs.ReadObject (reader);
            //Close readers
            reader.Close();
            fs.Close();

            //Writing back to collections
            //It is important that shapes are serialized first!
            foreach (Shape shape in m.countries)
            {
                shapes.Add(shape);
            }

            foreach (serialLine line in m.connections)
            {
                lines.Add(new Line(line, shapes));
            }
            Console.WriteLine(lines.Count);
            //End
        }
    }
}


/*
 *         public void Save(ObservableCollection<Shape> shapes)
        {
            //https://msdn.microsoft.com/en-us/library/ms752244(v=vs.110).aspx


            // For debug
            //var s1 = new Shape() { X = 123, Y = 69 };
            //Console.WriteLine(s1.ToString());
            // TODO: Allow to serialize multiple objects at once
            var writer = new FileStream("C:/Users/Martin/Datafile.xml", FileMode.Create);
            
            MemoryStream stream1 = new MemoryStream();

            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Shape));
            StreamWriter s = new StreamWriter(writer);
            s.AutoFlush = true;
            s.Write("<root>");

            for (int i = 0; i < shapes.Count; i++) {
                serializer.WriteObject(writer, shapes[i]);
            }


            //var ser = new DataContractSerializer(typeof(Shape));
            //ser.WriteObject(writer, s1);

            s.Write("</root>");

            writer.Close();
        }

        public void Load(ObservableCollection<Shape> shapes)
        {
            //var fs = new FileStream("C:/Users/Martin/Datafile.xml", FileMode.Open);
            //var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            //var ser = new DataContractSerializer(typeof(Shape));

            //for (int i = 0; i < 1; i++)
            //{
            //    var deserializedObjects = ser.ReadObject(reader, true);
            //    shapes.Add(deserializedObjects);
            //}

            XDocument doc = XDocument.Load("C:/Users/Martin/Datafile.xml");
            XmlSerializer xmls = new XmlSerializer(typeof(Shape));

            var _shapes = doc.Descendants("{http://schemas.datacontract.org/2004/07/Risk.Model}Shape");

            foreach (var shape in _shapes)
            {
                //Console.WriteLine(author.Value);
                StringReader r = new StringReader(shape.ToString());
                Shape s = (Shape)(xmls.Deserialize(r));
                shapes.Add(s);
                Console.WriteLine("Debug 1");
            }
            Console.ReadLine();

            // Deserialize the data and read it from the instance.
            //var deserializedObjects = (Shape)ser.ReadObject(reader, true);
            //reader.Close();
            //fs.Close();
            // TODO: Add the shape on the canvas
        }
        */
