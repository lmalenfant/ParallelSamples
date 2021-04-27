using System.Text.Json;

namespace ParallelSamples.IO
{
    public static class FileIo
    {
        public static T DuplicateObjectViaSerialization<T>(this T genericObject)
        {
            var serializedObject = JsonSerializer.Serialize(genericObject);
            return JsonSerializer.Deserialize<T>(serializedObject);
        }
    }
}
