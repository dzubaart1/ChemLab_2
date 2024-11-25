using System.IO;
using Newtonsoft.Json;

namespace JSONSaver
{
    public class JSONSaver
    {
        public static string SaveToFile<T>(T data, string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                using (File.Create(filePath))
                {
                }
            }

            using (StreamWriter outStream = new StreamWriter(filePath))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };

                string json = JsonConvert.SerializeObject(data, settings);
                
                outStream.Write(json);
                outStream.Close();

                return json;
            }
        }

        public static T LoadFromFile<T>(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                File.Create(filePath);
            }

            using (StreamReader outStream = new StreamReader(filePath))
            {
                var json = outStream.ReadToEnd();
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };

                outStream.Close();
                return (T)JsonConvert.DeserializeObject(json, settings);
            }
        }
    }
}
