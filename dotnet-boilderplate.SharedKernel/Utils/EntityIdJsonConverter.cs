using Newtonsoft.Json;

namespace dotnet_boilderplate.SharedKernel.Utils
{
    public class EntityIdJsonConverter : JsonConverter<EntityId>
    {
        public override EntityId? ReadJson(JsonReader reader, Type objectType, EntityId? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (hasExistingValue)
            {
                return EntityId.From(existingValue!.ToString());
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, EntityId? value, JsonSerializer serializer)
        {
            writer.WriteValue(value!.ToString())    ;
        }
    }
}
