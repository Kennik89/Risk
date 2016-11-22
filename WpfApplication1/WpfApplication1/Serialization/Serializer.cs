using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using Risk.Model;

namespace Risk.Serialization
{
    class Serializer
    {
        [STAThread]
        static void Serialize()
        {
            // Create a hashtable of values that will eventually be serialized.
            Shape shapes = new Shape(50, 50, 50, 50) {};

            // To serialize the hashtable (and its key/value pairs), 
            // you must first open a stream for writing.
            // Use a file stream here.
            FileStream fs = new FileStream("DataFile.soap", FileMode.Create);

            // Construct a SoapFormatter and use it 
            // to serialize the data to the stream.
            SoapFormatter formatter = new SoapFormatter();
            try
            {
                formatter.Serialize(fs, shapes);
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
