using System;
using System.Collections.Generic;
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
        [STAThread]
        static void Serialize()
        {
            Shape shape1 = new Shape(50, 50, 50, 50);
            Shape shape2 = new Shape(100, 100, 50, 50);
            Line lines = new Line {From = shape1, To = shape2};
            //Map map = new Map {connections = lines, countries = {shape1, shape2}};

            DataContractSerializer ser = new DataContractSerializer(typeof(Shape));
           // ser.WriteObject();

            // To serialize the hashtable and its key/value pairs,  
            // you must first open a stream for writing. 
            // In this case, use a file stream.
            FileStream fs = new FileStream("DataFile.dat", FileMode.Create);

            try
            {
               // formatter.Serialize(fs, ser);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
