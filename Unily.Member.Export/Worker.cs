using System.Diagnostics;
using ChoETL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Unily.Member.Export.Helpers;
using Unily.Member.Export.Lucene;
using Unily.Member.Export.Models;
using Unily.Member.Export.Services;

internal class Worker
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;
    private readonly IUnilyAuthTokenService _unilyAuthTokenService;

    public Worker(IConfiguration configuration, ILogger<Worker> logger, IUnilyAuthTokenService unilyAuthTokenService)
    {
        _configuration = configuration;
        _logger = logger;
        _unilyAuthTokenService = unilyAuthTokenService;
    }   

    public void DoWork()
    {
        try
        {
            var allUsers = GetAllUsers();

            using var parser = new ChoCSVWriter(_configuration["ExportFile"])
                .WithFirstLineHeader()
                .QuoteAllFields();

            parser.Write(allUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while exporting users {ExMessage}", ex.Message);
        }
    }
    
    private IEnumerable<UnilyUser> GetAllUsers()
    {
        return GetAllUsers(query => { });
    }
    
    public IEnumerable<UnilyUser> GetAllUsers(Action<LuceneFilterGroupBuilder> addFilters)
    {
        var users = new List<UnilyUser>();
        int? lastId = null;

        Console.WriteLine("Getting all users from Unily");

        IEnumerable<UnilyUser> getUsers() => GetUsers(query =>
        {
            addFilters(query);

            if (!lastId.HasValue)
                return;

            query.AddRangeFilter(UserProperties.Id, "0", lastId.Value.ToString());
        });

        IEnumerable<UnilyUser> usersPage = null;

        while ((usersPage = getUsers()).Any())
        {
            users.AddRange(usersPage);

            // Search range has to go lower than the last ID as it's inclusive
            lastId = users.Min(x => x.Id) - 1;
        }

        Console.WriteLine($"Found {users.Count.ToString("N0")} users in Unily");

        return users;
    }
    
    private IEnumerable<UnilyUser> GetUsers(Action<LuceneFilterGroupBuilder> addFilters = null, int? take = null)
    {
        var queryText = LuceneQueryBuilder.GetQueryText(query =>
        {
            //query.AddFilter(Constants.UserProperties.IsApproved, "true", LuceneQueryPartRule.MustMatch);

            addFilters?.Invoke(query);
        });

        var gqlModel = GqlHelper.GetSearchModel("GetUsers", new Dictionary<string, object>
        {
            { "queryText", queryText },
            { "sort", new { field = UserProperties.Id, direction = "desc" } },
            { "take", take ?? _configuration.GetSection("MaxGraphTake").Get<int>() }
        });

        var request = new RestRequest(_configuration["Routes:GqlSearch"], Method.Post);
        request.AddJsonBody(gqlModel);

        var unilyApiUri = _configuration["Unily:ApiSiteUrl"];
        
        var response = Request(new RestClient(unilyApiUri), request);
        var gqlSearchResponse = response.Result.DeserializeResponse<GqlSearchResponse>();

        return gqlSearchResponse.Data.Users.ByQueryText.Data.Select(x => new UnilyUser
        {
            Email = x.Email ?? "",
            Id = x.Id,
            UniqueId = x.Key ?? "",
            AccountName = x.LoginName ?? "",
            Discoverable = x.Properties.IsApproved.ParseStringToBoolean(),
            DisplayName = x.Properties.DisplayName ?? "",
            FirstName = x.Properties.FirstName ?? "",
            LastName = x.Properties.LastName ?? "",
        });
    }
    
    private async Task<RestResponse> Request(RestClient client, RestRequest request)
    {
        var authToken = await _unilyAuthTokenService.GetAuthToken();
        try
        {
            if (authToken != null)
                request.AddHeader("Authorization", $"{authToken.TokenType} {authToken.AccessToken}");
            
            var logMessage = $"{request.Method.ToString()} {request.Resource.TrimStart('/')}";
            
            var stopwatch = Stopwatch.StartNew();
            var raw = await client.ExecuteAsync(request);
            stopwatch.Stop();

            _logger.LogDebug($"{logMessage} returned HTTP {(int)raw.StatusCode} {raw.StatusCode} {stopwatch.ElapsedMilliseconds}ms");
            
            return raw;
        }
        catch (Exception exception)
        {
            throw new Exception($"An error occurred while querying Unily APIs: {exception.Message}", exception);
        }
    }
}