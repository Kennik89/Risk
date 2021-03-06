﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Model;

namespace Risk.LoadSave
{
    public class Serializer
    {
        public static Serializer Instance { get; } = new Serializer();

        [STAThread]
        public void Save(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines)
        {

            //https://msdn.microsoft.com/en-us/library/ms752244(v=vs.110).aspx about DataContract
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Map",
                Filter = "Risk files (.risk)|*.risk",
                DefaultExt = "risk",
                AddExtension = true
            };

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Length == 0)
            {
                return;
            }

            FileStream writer = new FileStream(saveFileDialog.FileName, FileMode.Create);
            
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Map));

            //Create a Map object with lines and shapes
            Map m = new Map();

            //Insert shapes to map
            if (shapes.Count > 0)
            {

                foreach (Shape shape in shapes)
                {
                    m.Countries.Add(shape);
                }

            }

            //Insert lines to map
            if (lines.Count > 0)
            {
                foreach (Line line in lines)
                {
                    m.Connections.Add(new serialLine(line));
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
            var m = (Map) dcs.ReadObject (reader);
            //Close readers
            reader.Close();
            fs.Close();

            foreach (Shape shape in m.Countries)
            {
                shapes.Add(shape);
            }

            foreach (serialLine line in m.Connections)
            {
                lines.Add(new Line(line, shapes));
            }
            
            Console.WriteLine(lines.Count);
            return true;
        }
    }
}