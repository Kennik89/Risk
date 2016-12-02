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
using System.Windows.Forms;

namespace Risk.LoadSave
{
    public class _serializer
    {
        public static _serializer Instance { get; } = new _serializer();

        [STAThread]
        public void Save(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines)
        {
            /*
             * System.Threading.Thread.Sleep(5000);
            Console.WriteLine("Save-thread is running");
            */
            //https://msdn.microsoft.com/en-us/library/ms752244(v=vs.110).aspx
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Save Map";
            saveFileDialog.Filter = "Risk files (.risk)|*.risk";
            saveFileDialog.DefaultExt = "risk";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();

            FileStream writer = new FileStream(saveFileDialog.FileName, FileMode.Create);
            
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Map));

            //Maybe make all this until writing in a different thread since savefiledialog may block?

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

        //Return value is added to ensure that undo-redo stack is cleared in mainview-model
        //Without bool return, undo-redo had to be a parameter.
        public bool Load(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines)
        {
            //http://stackoverflow.com/questions/16943176/how-to-deserialize-xml-using-datacontractserializer
            //Define a map
            Map m;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "risk";
            ofd.SupportMultiDottedExtensions = false;
            ofd.Title = "Load Map";
            ofd.Filter = "Risk Files (.risk)|*.risk";


            ofd.ShowDialog();

            if (ofd.FileName.Length == 0)
            {
                return false;
            }
            shapes.Clear();
            lines.Clear();
            //Read from file 
            DataContractSerializer dcs = new DataContractSerializer(typeof(Map));
            FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
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
            //Console.WriteLine(lines.Count);
            //End
            return true;
        }
    }
}