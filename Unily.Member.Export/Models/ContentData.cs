using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class ContentData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("properties")]
    public ContentPropertyCollection Properties { get; set; }
}