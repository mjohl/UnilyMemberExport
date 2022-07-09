using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class UserData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("loginName")]
    public string LoginName { get; set; }

    [JsonProperty("properties")]
    public UserPropertyCollection Properties { get; set; }
}