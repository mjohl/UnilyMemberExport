using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class GqlSearchResponse
{
    [JsonProperty("data")]
    public Data Data { get; set; }
}