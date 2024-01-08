using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Agent
{
    public static class Extensions
    {
        public static byte[] Serialize<T>(this T data)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, data);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] data)
        {
            var deserializer = new DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                return (T) deserializer.ReadObject(ms);
            }
        }
    }
}
