using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /* Helper class that can serialize and deserialize 
     * restaurant and review objects in JSON */
    public class HelperSerialize<T>
    {
        public string getFilePath()
        {
            string filePath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName, "restaurants.json");
            return filePath;
        }
            
        public void SerializeRestaurants(List<T> restaurants, string filePath)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(restaurants, settings);
            File.WriteAllText(filePath, json);
        }

        public List<T> DeserializeRestaurants(string filePath)
        {
            var restaurantList = new List<T>();
            var searilizer = new JsonSerializer();
            using (var fileReader = new StreamReader(filePath))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                restaurantList = searilizer.Deserialize<List<T>>(jsonReader);
            }
            return restaurantList;
        }
    }
}
