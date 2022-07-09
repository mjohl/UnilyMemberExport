using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class ContentByQueryText
{
    [JsonProperty("totalRows")]
    public int TotalRows { get; set; }

    [JsonProperty("data")]
    public IEnumerable<ContentData> Data { get; set; } = Enumerable.Empty<ContentData>();
}