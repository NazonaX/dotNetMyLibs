using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MapAndSimulation.Map;

namespace MapAndSimulation.Utils
{
    public static class IOOps
    {
        public static void ClassWrite(string path, Object obj)
        {
            if (obj == null || "".Equals(path))
                return;
            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, obj);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw ex;
                }
            }
        }

        public static Object ClassRead(string path)
        {
            Object obj = null;
            if ("".Equals(path))
                return obj;
            using(FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                obj = bf.Deserialize(fs);
            }
            return obj;
        }

        public static bool IsFileExisted(string mapFile)
        {
            if ("".Equals(mapFile))
                return false;
            return File.Exists(mapFile);
        }

        public static void AppendTxt(string path, string msg)
        {
            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine(msg);
                    sw.Flush();
                }
            }
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static object CopyMemory(object obj)
        {
            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, obj);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream);
            }
        }
    }
}
