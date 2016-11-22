using System;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using Risk.Command;
using Risk.Model;

namespace Risk.Serialization
{
    class Serializer
    {
        [STAThread]
        static void Serialize()
        {
            // Create a hashtable of values that will eventually be serialized.
            ObservableCollection<Shape> Shapes = new ObservableCollection<Shape>();

            FileStream fs = new FileStream("DataFile.soap", FileMode.Create);

            // Construct a SoapFormatter and use it 
            // to serialize the data to the stream.
            SoapFormatter formatter = new SoapFormatter();
            try
            {
                formatter.Serialize(fs, Shapes);
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
