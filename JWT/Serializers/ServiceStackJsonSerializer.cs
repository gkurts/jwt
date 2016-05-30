namespace JWT.Serializers
{
    public class ServiceStackJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }

        public T Deserialize<T>(string json)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(json);
        }
    }
}