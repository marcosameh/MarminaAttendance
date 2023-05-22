using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.Utilities
{
    public static class CollectionUtilities
    {
        ///
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection)
        {
            return collection.Select((item, index) => (item, index));
        }

        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        public static void Put<T>(this ISession session, string key, T value) where T : class
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            session.SetString(key, serializedObject);
        }

        public static T Get<T>(this ISession session, string key) where T : class
        {
            var deSerializedObject = session.GetString(key);
            return JsonConvert.DeserializeObject<T>(deSerializedObject);
        }

        public static string GetVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "";
            var uri = new Uri(url);

            // you can check host here => uri.Host <= "www.youtube.com"
            var query = HttpUtility.ParseQueryString(uri.Query);
            string videoId = string.Empty;
            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }
            return videoId;
        }

        private static Random random = new Random();
        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                throw new ArgumentException("Array must not be empty.");
            }

            int randomIndex = random.Next(array.Length);
            return array[randomIndex];
        }
    }


}
