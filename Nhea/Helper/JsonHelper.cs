using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace Nhea.Helper
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string payload)
        {
            return JsonConvert.DeserializeObject<T>(payload);
        }

        public static T DeserializeObject<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JObject.FromObject(obj).ToString());
        }

        public static T Merge<T>(T source, object add)
        {
            if (add != null)
            {
                var sourceObject = JObject.FromObject(source);
                var addObject = JObject.FromObject(add);
                sourceObject.Merge(addObject, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });

                return DeserializeObject<T>(sourceObject);
            }
            else
            {
                return source;
            }
        }

        public static string SerializeObject(object obj)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }

            return sb.ToString();
        }
    }
}