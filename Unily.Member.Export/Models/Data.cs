using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class Data
{
    [JsonProperty("users")]
    public Users Users { get; set; }

    [JsonProperty("content")]
    public Content Content { get; set; }
}