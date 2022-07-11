# Unily Member Export

This project shows how you can use the GQL Search API to export members from Unily.
To be able to run this project against your Unily instance, you need to have a valid API key and secret.
The project uses .Net Core 6 and the Unily API.

To find out more details about the API, visit the Universe (requires access) documentation [here](https://universe.unily.com/sites/developer-guides/tech-guide/12246/unily-apis)

The project uses a GQL query to extract all members from the site and exports the data defined in the GQL query to a CSV file.
The code can easily be adapted to do other operations such as updating members or content. The scope defined in the settings also shows how you can use multiple scopes, just ensure that the registered API application has the same scopes.

Unily tokens are only valid for an hour and in some instances you may require to run the application for longer. The console application will automatically refresh the token if it expires.

## How to run
You need to update the configuration values in the appsettings.json file.

```
{
    "ExportFile": "D:\\Export\\AllMembers.csv",
    "MaxGraphTake": 10000,
    "Unily": {
        "ApiSiteUrl": "https://xxxxxx-api.unily.com/",
        "IdentityServiceUri": "https://xxxxxxx-idsrv.unily.com/",
        "API": {
            "ClientId": "a7cb7171-xxxxxxxxxxxxx-9e27506020c7",
            "ClientSecret": "mGAtxxxxxxxxxxxxxIgjtg",
            "Scopes": [
                "gateway.graphql"
            ]
        }
    },
    "Routes": {
        "GqlSearch": "/api/v1/search",
        "UserDeleteBulk": "/api/v1/users"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    }
}

```

Then you can run the project.
If you want to change the GQL Query you can update the **Helpers\GqlQueries.cs** file, just remember to also update the UnilyUser model as well.
