using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class Content
{
    [JsonProperty("byQueryText")]
    public ContentByQueryText ByQueryText { get; set; }
}