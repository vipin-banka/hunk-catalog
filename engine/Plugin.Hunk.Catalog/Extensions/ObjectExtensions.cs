using Newtonsoft.Json;

namespace Plugin.Hunk.Catalog.Extensions
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T value) where T : class
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }
    }
}