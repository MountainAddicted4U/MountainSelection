using MountainAddicted.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MountainAddicted.Library
{
    public class DataConverter
    {
        public static string ConvertDataToString(MountainData data)
        {
            if(data == null)
            {
                return null;
            }

            var xmlSerializer = new XmlSerializer(typeof(MountainData));
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }
        public static MountainData ConvertStringToData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var xmlSerializer = new XmlSerializer(typeof(MountainData));
            using (var textReader = new StringReader(data))
            {
                return (MountainData)xmlSerializer.Deserialize(textReader);
            }
        }
    }
}
