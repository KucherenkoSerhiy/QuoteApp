using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using QuoteApp.Backend.BusinessLogic.Manager;

namespace QuoteApp.Globals
{
    public class QuoteAppUtils
    {
        public static void SerializeToXml(object instance, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());

                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                writer.WriteStartDocument();

                serializer.Serialize(writer, instance);

                writer.WriteEndDocument();
                writer.Close();
            }
        }

        public static T DeserializeXml<T>(string xmlFilePath)
        {
            string xmltext = File.ReadAllText(xmlFilePath);

            XmlReader xReader = XmlReader.Create(new StringReader(xmltext));
            var deserializer = new XmlSerializer(typeof(T));
            var deserializedData = (T)deserializer.Deserialize(xReader);

            return deserializedData;
        }

        /// <summary>
        /// Reads file located in project directory
        /// </summary>
        /// <param name="namespacePlusFileName">Must specify file location + namespace. Example: MyProject.MyFolder.MyFile</param>
        /// <returns>file content splitted by lines</returns>
        public static string[] ReadLocalFile(string namespacePlusFileName)
        {
            var assembly = typeof(SqliteDbManager).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(namespacePlusFileName);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return lines;
        }
    }
}
