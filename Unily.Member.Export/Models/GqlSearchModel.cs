using Newtonsoft.Json;

namespace Unily.Member.Export.Models;

internal class GqlSearchModel
{
    [JsonProperty("operationName")]
    public string OperationName { get; set; }

    [JsonProperty("namedQuery")]
    public string NamedQuery { get; set; }

    [JsonProperty("query")]
    public string Query { get; set; }

    //{
    //  "operationName": "example",
    //  "namedQuery": "",
    //  "query": "query example{content{list{data{id}}}}"
    //}
}