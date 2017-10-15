using System;
using System.Linq;
using System.LocalCore.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading;
using LocalCore.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using tokenvalidator;

namespace tokenvalidatorcore
{
    internal static class Program
    {
        private static void Main()
        {
            const string domain = "https://www.test.vegvesen.no:443/openam/oauth2/employees";
            const string audience = "ComAround";
            const string token = "eyJ0eXAiOiJKV1QiLCJraWQiOiJDanhYcUhFYlpvWEdYNkRHTE4xZzRaY1hCWTQ9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJURVNUMTAiLCJhdWRpdFRyYWNraW5nSWQiOiIzZDJkNTVlZS1hNmEzLTRmNDgtYjM3MC0yZDkwNmM2MzA4NGQiLCJpc3MiOiJodHRwczovL3d3dy50ZXN0LnZlZ3Zlc2VuLm5vOjQ0My9vcGVuYW0vb2F1dGgyL2VtcGxveWVlcyIsInRva2VuTmFtZSI6ImFjY2Vzc190b2tlbiIsInRva2VuX3R5cGUiOiJCZWFyZXIiLCJhdXRoR3JhbnRJZCI6IjFkN2M0OWQ0LWM4OTItNDYxZS1iMGQ2LWIzZDNjMDk4ZDZiZSIsImF1ZCI6IkNvbUFyb3VuZCIsIm5iZiI6MTUwODAyMjM0Niwic2NvcGUiOlsic3Z2cHJvZmlsZSIsIm9wZW5pZCJdLCJhdXRoX3RpbWUiOjE1MDgwMjIzNDYsInJlYWxtIjoiL0VtcGxveWVlcyIsImV4cCI6MTUwODA1MTE0NiwiaWF0IjoxNTA4MDIyMzQ2LCJleHBpcmVzX2luIjoyODgwMDAwMCwianRpIjoiNzE3Yzk0YTYtNDdmOS00MzlhLWEwNzQtMjI4MjYyMTBiMzc5In0.kY2T0pAWopjsOmajjscXeI8HgrrQ8-e6LZprn9_LgSPRfYq5crUMxJ70rLawEMnpIkv7KEvNO3yfJNYTbif3miztbY5ipEN9CHE-2UBZqTv_A5i2-lvq52UDo4D10BL45j3ULW4VpqI_hu_JWCRxJrE5lt4AoLRywWJvdIHuaQ49krzANb2pzvZT0mwVwB0XfiwmwHCZ8uHAhVaxx9JtdiDyhfpLBstqjUpp1tfpJRpzjoqCcvKeUM7FmtnUnG6QMTTerixqBSveKEjL2naKnF7h7m4p6WjsA5POYK3Dpra12SPQ-iflr_8MbcIyVxMtj_yhCdUWAglHOPoxb0RG7Q";

            //const string domain = "https://testlogin.comaround.com/";
            //const string token = "";
            //const string audience = "https://testlogin.comaround.com/resources";

            try
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain}/.well-known/openid-configuration",new OpenIdConnectConfigurationRetriever());
                var configuration = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = domain,
                    ValidAudiences = new[] { audience },
                    IssuerSigningKeys = configuration.SigningKeys
                };
                var handler = new JwtSecurityTokenHandler();
                var user = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                Console.WriteLine($"Token {validatedToken.Id} is validated. User Id {user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while validating token: {e.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}
