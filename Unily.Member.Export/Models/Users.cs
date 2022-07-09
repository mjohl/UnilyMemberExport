using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class Users
{
    [JsonProperty("byQueryText")]
    public UsersByQueryText ByQueryText { get; set; }
}