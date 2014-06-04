using System;
using System.IO;
using System.Text;
using Griffin.Net.Protocols.MicroMsg;
using Newtonsoft.Json;

namespace MicroServices.Proxy
{
    public class JsonSerializer : IMessageSerializer
    {
        public void Serialize(object source, Stream destination, out string contentType)
        {
            contentType = "application/json";
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(source, settings));
            destination.Write(data, 0, data.Length);
        }

        public object Deserialize(string contentType, Stream source)
        {
            var data = Encoding.UTF8.GetString(ReadFully(source));
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            return JsonConvert.DeserializeObject(data, Type.GetType(contentType), settings);
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}