using Newtonsoft.Json;
using Unily.Member.Export.Models;

namespace Unily.Member.Export.Helpers;

internal static class GqlHelper
{
    const string ParametersToken = "$parameters$";

    internal static string GetGqlQuery(string name, Dictionary<string, object> parameters = null)
    {
        var queryLines = GqlQueries.GetQueryLines(name);

        // Flatten and remove indent
        var query = string.Join(" ", queryLines.Select(x => x.TrimStart(' ')));

        if (!query.Contains(ParametersToken))
            return query;

        parameters = parameters ?? new Dictionary<string, object>();

        var gqlParameters = parameters.Select(x => $"{x.Key}: {SerializeObject(x.Value)}");
        var gqlParameterString = string.Join(", ", gqlParameters);

        return query.Replace(ParametersToken, gqlParameterString);
    }

    /// <summary>
    /// Will serialize objects to JSON, without quoting the property names - creates valid GQL objects
    /// </summary>
    private static string SerializeObject(object value)
    {
        using (var stringWriter = new StringWriter())
        {
            using (var writer = new JsonTextWriter(stringWriter))
            {
                writer.QuoteName = false;

                var serializer = new JsonSerializer();
                serializer.Serialize(writer, value);
            }

            return stringWriter.ToString();
        }
    }

    internal static GqlSearchModel GetSearchModel(string queryName, Dictionary<string, object> parameters = null)
    {
        var query = GetGqlQuery(queryName, parameters);

        return new GqlSearchModel
        {
            OperationName = queryName,
            Query = $"query {queryName} {query}"
        };
    }
}