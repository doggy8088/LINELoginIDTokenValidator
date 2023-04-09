using System.CommandLine;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

var tokenOption = new Option<string>(
            name: "--token",
            description: "The LINE Login's ID Token.");

var channelIdOption = new Option<string>(
            name: "--channel-id",
            description: "The LINE Login's Channel ID.");

var rootCommand = new RootCommand("LINE Login ID Token Validator");
rootCommand.AddOption(tokenOption);
rootCommand.AddOption(channelIdOption);

rootCommand.SetHandler(async (idToken, channelId) =>
    {
        var certs = await GetJWKsCerts();

        var keys  = new JsonWebKeySet(certs).GetSigningKeys();

        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(idToken);

        var key = keys.FirstOrDefault(k => k.KeyId == decodedToken.Header.Kid);

        var handler = new JsonWebTokenHandler();
        TokenValidationResult result = handler.ValidateToken(idToken, new TokenValidationParameters
        {
            ValidIssuer = "https://access.line.me",
            ValidAudience = channelId, // LINE Login 的 Channel ID
            IssuerSigningKey = key,
        });

        if (result.IsValid)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The ID Token is VALID.");
            Console.ResetColor();
        }
        else
        {
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            // console output with color red
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The ID Token is INVALID: " + result.Exception.Message);
            Console.ResetColor();
        }
    },
    tokenOption, channelIdOption);

return await rootCommand.InvokeAsync(args);

// ----------------------------------------------

static async Task<string> GetJWKsCerts()
{
    var http = new HttpClient();

    // dotnet add package IdentityModel
    var disco = await http.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
    {
        Address = "https://access.line.me", // Authority
        Policy = new() { ValidateEndpoints = false }
    });

    var certs = await http.GetStringAsync(disco.JwksUri);

    return certs;
}