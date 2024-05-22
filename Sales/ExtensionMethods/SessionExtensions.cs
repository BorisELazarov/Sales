using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace Sales.ExtensionMethods
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession instance, string key, T value)
            where T : class
        {
            if (value == null)
            {
                instance.Remove(key);
                return;
            }
            string jsonData=JsonSerializer.Serialize(value);
            instance.SetString(key, jsonData);
        }

        public static T GetObject<T>(this ISession instance, string key)
            where T : class
        {
            if (!instance.Keys.Contains(key))
                return null;

            string jsonData = instance.GetString(key);

            if (string.IsNullOrEmpty(jsonData))
                return null;

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
