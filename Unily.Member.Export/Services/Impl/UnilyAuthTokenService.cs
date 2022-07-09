using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Unily.Member.Export.Models;

namespace Unily.Member.Export.Services.Impl;

public class UnilyAuthTokenService: IUnilyAuthTokenService
{
    private static readonly Dictionary<string, AuthTokenResponse> tokens = new ();
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public UnilyAuthTokenService(ILogger logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<AuthTokenResponse?> GetAuthToken()
    {
        var clientId = _configuration["Unily:API:ClientId"];
        var clientSecret = _configuration["Unily:API:ClientSecret"];
        var scope = String.Join(" ", _configuration.GetSection("Unily:API:Scopes").Get<string[]>());
        var identityServiceUri = _configuration["Unily:IdentityServiceUri"];
        
        var tokenKey = clientId;
        if (!string.IsNullOrEmpty(scope))
            tokenKey += $" {scope}";

        var tokenExists = tokens.TryGetValue(tokenKey, out var authToken);

        if (tokenExists && authToken.ExpiryDate > DateTime.UtcNow) 
            return authToken;
        
        _logger.LogDebug("Acquiring new {TokenKey} auth token:", tokenKey);

        var authRequest = new RestRequest("/connect/token", Method.Post);
        authRequest.AddParameter("grant_type", "client_credentials");
        authRequest.AddParameter("client_id", clientId);
        authRequest.AddParameter("client_secret", clientSecret);

        if (!string.IsNullOrEmpty(scope))
            authRequest.AddParameter("scope", scope);

        var authClient = new RestClient(identityServiceUri);
        var authResponse = await authClient.ExecuteAsync(authRequest);
        
        authToken = JsonConvert.DeserializeObject<AuthTokenResponse>(authResponse.Content);
            
        _logger.LogDebug("{AuthTokenTokenType} token acquired", authToken?.TokenType);
        _logger.LogDebug(authToken?.AccessToken);

        tokens[tokenKey] = authToken;

        return authToken;
    }
}