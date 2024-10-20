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

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## Disclaimer
The code provided in this project is **not developed, endorsed, or supported by Unily**. It is intended as an example of how to use Unily's APIs and GraphQL queries for content export, but **you are solely responsible for testing and validating its functionality** in your own environment.

By using this code, you acknowledge and agree that:

- **No warranty or liability** is provided. The project is offered "as-is," with no guarantees of accuracy, reliability, or performance.
- **Unily is not liable** for any issues or damages that arise from the use of this code.
- You are responsible for ensuring compliance with any relevant legal agreements or terms of service associated with Unily's APIs and any data processed through them.
- This tool **should not be used in a production environment** without thorough testing and review, as it may not cover all use cases or error conditions.
- **Modifications or extensions** to this code should be handled with caution and may require additional testing or adjustments based on specific requirements.
- It is your responsibility to ensure **data privacy and security** when exporting or handling any content.

Please note that this repository is **not actively monitored**, and no direct support is available. You are encouraged to fork the project and make any necessary adjustments for your own use.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
