using RestSharp;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Unily.Member.Export.Helpers;

public static class IRestResponseExtensions
{
    public static T DeserializeResponse<T>(this RestResponse response)
    {
        var deserializedValue = default(T);
        if (string.IsNullOrEmpty(response?.Content))
            return deserializedValue;

        deserializedValue = response.Content.DeserializeJson<T>();

        if (deserializedValue?.Equals(default(T)) ?? true)
        {
            var type = typeof(T);
            var typeName = type.Name;
            if (type.IsGenericType)
            {
                typeName += "<";
                typeName += string.Join(",", type.GetGenericArguments().Select(x => x.Name));
                typeName += ">";
            }

            throw new Exception($"Could not deserialize value to {typeName}: {response?.Content}");
        }

        return deserializedValue;
    }

    private static readonly Regex RFC5988 = new Regex("<(.+)>; rel=\"([a-z]+)\"", RegexOptions.Compiled);

    // http://tools.ietf.org/html/rfc5988
    public static Uri GetLinkHeader(this RestResponse response, string rel)
    {
        // Case-insensitive match on "link" as per https://tools.ietf.org/html/rfc8288#appendix-B.1
        var linkHeader = response.Headers.SingleOrDefault(x => x.Name.Equals("Link", StringComparison.OrdinalIgnoreCase));
        if (linkHeader == null)
            return null;

        if (!(linkHeader.Value is string value))
            return null;

        var links = value.Split(',');
        foreach (var link in links)
        {
            var match = RFC5988.Match(link);
            if (!match.Success)
                continue;

            if (match.Groups[2].Value == rel)
                return new Uri(match.Groups[1].Value);
        }

        return null;
    }
}