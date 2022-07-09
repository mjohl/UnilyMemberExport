using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

public class AuthTokenResponse
{
    private readonly DateTime created;

    public AuthTokenResponse()
    {
        created = DateTime.UtcNow;
    }

    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    private int expiresIn;
    [JsonProperty("expires_in")]
    public int ExpiresIn
    {
        get
        {
            return expiresIn;
        }
        set
        {
            expiresIn = value;
            TokenLife = TimeSpan.FromSeconds(expiresIn);
            ExpiryDate = created + TokenLife;
        }
    }

    public TimeSpan TokenLife { get; private set; }

    public DateTime ExpiryDate { get; private set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}