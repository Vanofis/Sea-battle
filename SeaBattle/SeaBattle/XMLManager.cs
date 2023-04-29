using System.IO;
using System.Xml.Serialization;

namespace SeaBattle
{
    public static class XMLManager
    {
        public static string pathToProfiles { get; private set; } = Directory.GetCurrentDirectory() + $@"\Profiles";

        public static void SerializeXML(PlayerProfile playerProfile, string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayerProfile));

            string path = pathToProfiles + @$"\{fileName}";

            using (StreamWriter writer = new StreamWriter(path))
            {
                try
                {
                    xmlSerializer.Serialize(writer, playerProfile);
                }
                finally
                {
                    writer.Close();
                }
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
                PlayerProfile deserialize = null;

                try
                {
                    deserialize = (PlayerProfile)xmlSerializer.Deserialize(reader);
                }
                finally
                {
                    reader.Close();
                }

                return deserialize;
            }
        }
    }
}
