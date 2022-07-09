using RestSharp;
using Unily.Member.Export.Models;

namespace Unily.Member.Export.Services;

public interface IUnilyAuthTokenService
{
    public Task<AuthTokenResponse?> GetAuthToken();
}