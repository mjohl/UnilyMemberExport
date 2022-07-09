using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Unily.Member.Export.Services;

internal class Worker
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;

    public Worker(IConfiguration configuration, ILogger<Worker> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }   

    public void DoWork()
    {
        var keyValuePairs = _configuration.AsEnumerable().ToList();

        foreach (var pair in keyValuePairs)
        {
            _logger.LogInformation($"{pair.Key} - {pair.Value}");
        }
    }
}