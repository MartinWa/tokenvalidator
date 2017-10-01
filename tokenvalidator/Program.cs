using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.IdentityModel.Protocols;

namespace tokenvalidator
{
    internal static class Program
    {
        private static void Main()
        {
            const string domain = "https://www.test.vegvesen.no:443/openam/oauth2/employees";
            const string audience = "ComAround";
            const string token = "";
            
            //const string domain = "https://testlogin.comaround.com/";
            //const string token = "";
            //const string audience = "https://testlogin.comaround.com/resources";

            try
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain}/.well-known/openid-configuration");
                var configuration = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = domain,
                    ValidAudiences = new[] { audience },
                    IssuerSigningToken = configuration.SigningTokens.FirstOrDefault(x => x is NamedKeySecurityToken)
                    //    IssuerSigningTokens = configuration.SigningTokens
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
