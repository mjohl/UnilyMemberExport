using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class UsersByQueryText
{
    [JsonProperty("totalRows")]
    public int TotalRows { get; set; }

    [JsonProperty("data")]
    public IEnumerable<UserData> Data { get; set; } = Enumerable.Empty<UserData>();
}