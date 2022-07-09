namespace Unily.Member.Export.Helpers;

internal static class GqlQueries
{
    // I hate this as much as you do. Necessary beast though.
    internal static List<string> GetQueryLines(string query)
    {
        string queryText;
        switch (query)
        {
            case "GetUsers":
            {
                queryText = @"{
                                users {
                                byQueryText($parameters$) {
                                            totalRows,
                                            data {
                                                email,
                                                id,
                                                key,
                                                loginName,
                                                properties
                                                {
                                                    ... on BaseMemberInterface
                                                    {
                                                        personalAppIds,
                                                        umbracoMemberApproved
                                                    }
                                                    ... on Member
                                                    {
                                                        displayName,
                                                        firstName, 
                                                        lastName, 
                                                        personalAppIds
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }";
            }
                break;
            default:
                throw new NotImplementedException();
        }

        return new List<string> { queryText };
    }
}