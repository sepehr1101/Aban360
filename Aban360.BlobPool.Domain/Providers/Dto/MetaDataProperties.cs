using System.Text.Json.Serialization;

namespace Aban360.BlobPool.Domain.Providers.Dto
{
    public class MetaDataProperties
    {
        [JsonPropertyName("simplePropertyGroup")]
        public List<PropertyGroupItem>? RawMetaDatas { get; set; }
    }
    public class MetaDataProperty
    {
        [JsonPropertyName("simplePropertyGroup")]
        public PropertyGroupItem? RawMetaDatas { get; set; }
    }
    public class PropertyGroupItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("value")]
        public int? Value { get; set; }= default!;
    }
    public class PropertyGroupContainer
    {
        [JsonPropertyName("simplePropertyGroup")]
        public List<PropertyGroupItem> SimplePropertyGroup { get; set; }
    }
}
