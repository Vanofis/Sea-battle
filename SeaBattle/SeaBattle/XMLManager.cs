using System;
using System.IO;
using System.Xml.Serialization;

namespace SeaBattle
{
    public static class XMLManager
    {
        public static string pathToProfiles { get; private set; } = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + $@"\Profiles";

        public static void SerializeXML(PlayerProfile playerProfile, string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile));

            string path = pathToProfiles + @$"\{fileName}";

            using (StreamWriter writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, playerProfile);
            }
        }
        public static PlayerProfile DeserializeXML(string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile));

            string path = pathToProfiles + $@"\{fileName}";

            if (!File.Exists(path))
                return null;

            using (StreamReader reader = new StreamReader(path))
            {
                var deserialize = (PlayerProfile)xmlSerializer.Deserialize(reader);

                return deserialize;
            }
        }
    }
}
