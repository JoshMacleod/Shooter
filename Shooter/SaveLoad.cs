using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Shooter
{
    class SaveLoad
    {
        public static void SaveData(object obj, string filename)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }

        public static GameData LoadData(string filename)
        {
            XmlSerializer de = new XmlSerializer(typeof(GameData));
            FileStream read = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            GameData obj = (GameData)de.Deserialize(read);
            return obj;
        }
    }
}
