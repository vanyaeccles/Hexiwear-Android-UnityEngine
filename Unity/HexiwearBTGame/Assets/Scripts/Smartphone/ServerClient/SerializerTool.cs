using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ServerClient
{
    static class SerializerTool
    {
        public static T DeserializeObject<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(toDeserialize);
            return (T)(xmlSerializer.Deserialize(textReader));
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    }
}
