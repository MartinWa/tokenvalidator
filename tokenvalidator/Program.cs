using System;
using System.Collections.Generic;
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
            const string token = "eyJ0eXAiOiJKV1QiLCJraWQiOiJDanhYcUhFYlpvWEdYNkRHTE4xZzRaY1hCWTQ9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJURVNUMTAiLCJhdWRpdFRyYWNraW5nSWQiOiJiYjA1OWRmZS03OGEwLTRmMmItYTNhZC02NGE2N2Q2YmQxYjYiLCJpc3MiOiJodHRwczovL3d3dy50ZXN0LnZlZ3Zlc2VuLm5vOjQ0My9vcGVuYW0vb2F1dGgyL2VtcGxveWVlcyIsInRva2VuTmFtZSI6ImFjY2Vzc190b2tlbiIsInRva2VuX3R5cGUiOiJCZWFyZXIiLCJhdXRoR3JhbnRJZCI6ImQ4NmQxODFhLTUzNDctNDFhOS1hNTkzLTA3MzJhZWUwMDYxZiIsImF1ZCI6IkNvbUFyb3VuZCIsIm5iZiI6MTUwNzk5MTA3Niwic2NvcGUiOlsic3Z2cHJvZmlsZSIsIm9wZW5pZCJdLCJhdXRoX3RpbWUiOjE1MDc5OTEwNzYsInJlYWxtIjoiL0VtcGxveWVlcyIsImV4cCI6MTUwODAxOTg3NiwiaWF0IjoxNTA3OTkxMDc2LCJleHBpcmVzX2luIjoyODgwMDAwMCwianRpIjoiYWIzM2M2ZTYtMTgzZC00MzAxLWJjNTQtMmZlNWU0YmQyYTJmIn0.PhPluEwRQ3ggkxqzReRyWzztF00tQFqfwPt4ENH0PFPCSI_-Kt3148-uCUsCpe5mpkoRaMnhiuYacX5qZ-0vK6d1oPNIcxwbhqmqN3XXF07YwTn196K7bDS9xxIDXHQ8bynlqeRY6xNNfT9__vQwDYwrz_R2BdjePJnGN2GDRujY7VKwel32T4Lb7XG4Q6mpzPzO0A8BmXeUxUObpb5P2Uzkav-lMNGonucxKOcLEWEVt0-zaZnyibS5Ps6DENccVsnLXDMXXI7o8UBiQ46DCSTgwi4drdg4YfMMvITl_wMo4bt_xXPJ3_mdR8VRJS11hzsNfQWfiVJWz-kRGO7s3Q";

            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain}/.well-known/openid-configuration");
            var configuration = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));
            var issuerSigningToken = configuration.SigningTokens.FirstOrDefault(x => x is NamedKeySecurityToken);
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = domain,
                ValidAudience = audience,
                IssuerSigningToken = issuerSigningToken
                //   IssuerSigningTokens = configuration.SigningTokens,
                //     IssuerSigningKeyValidator = IssuerSigningKeyValidator,
                //    IssuerValidator = IssuerValidator,
                //    AudienceValidator = AudienceValidator,
                //    LifetimeValidator = LifetimeValidator,
                //        IssuerSigningKeyResolver = IssuerSigningKeyResolver,

            };
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var user = handler.ValidateToken(token, validationParameters, out var validatedToken);
                Console.WriteLine($"Token {validatedToken.Id} is validated. User Id {user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while validating token: {e.Message}");
            }

            const string domain2 = "https://testlogin.comaround.com";
            const string token2 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyIsImtpZCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyJ9.eyJpc3MiOiJodHRwczovL3Rlc3Rsb2dpbi5jb21hcm91bmQuY29tLyIsImF1ZCI6Imh0dHBzOi8vdGVzdGxvZ2luLmNvbWFyb3VuZC5jb20vcmVzb3VyY2VzIiwiZXhwIjoxNTA3OTk1NDQyLCJuYmYiOjE1MDc5OTE4NDIsImNsaWVudF9pZCI6Inplcm9mcm9udGVuZCIsInNjb3BlIjoiemVyb2FwaSIsInN1YiI6IjYxOTAiLCJhdXRoX3RpbWUiOjE1MDc5OTE4MzgsImlkcCI6Imlkc3J2IiwibmFtZSI6Ik1hcnRpbiBXw6VnZXIiLCJhbGxvd2VkaXAiOiIqIiwiY3VzdG9tZXJpZCI6IjEzOSIsImVtYWlsIjoibWFydGluLndAY29tYXJvdW5kLnNlIiwidXNlcnVnYW0iOiIxNjQxNyIsImxpY2Vuc2UiOiJCYXNpYyIsInBvcnRhbGlkcyI6WyIxOTciLCIyMDEiLCI5NzgiXSwicm9sZSI6Ik1hbmFnZXIiLCJ1c2VyZ3JvdXBpZHMiOlsiMTQyNSIsIjE0NDUiLCIzOTA3Il0sImFtciI6WyJQYXNzd29yZCJdfQ.g9K5vyagFoy7VjNTtSldtGwq-JrOTeTQu6pAKpe67UevhDBDumgdctfZLHKL3tXk4gCWAnWfKGOQe69khLahcEzmZJUJrtU1ToJkrRMj2vD_PPYD4ehnOllYPCiAqgbkGknIbDpWdvzCFRTjbtC3LeHoW0-0uV5yg_CXAFbopWeHxc7F7XZarG4Gcv3gR_nkhxVEakLBlvGhMlrKLzUWHrm7mxoTg5IWFEcALSaHV6VkEEMkoboxMRW3wmJMgay1yDChbgs8IcLKKPqG76Y59UBFWJRr4f3Kh0KZmd42Bf8gR6odf0wbWW7ISEui8piE4JzSLgjLRrDi4YZJoL3UKQ";
            const string audience2 = "https://testlogin.comaround.com/resources";

            var configurationManager2 = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain2}/.well-known/openid-configuration");
            var configuration2 = AsyncHelper.RunSync(async () => await configurationManager2.GetConfigurationAsync(CancellationToken.None));
            var issuerSigningToken2 = configuration2.SigningTokens.FirstOrDefault(x => x is NamedKeySecurityToken);
            var validationParameters2 = new TokenValidationParameters
            {
                ValidIssuer = domain2,
                ValidAudience = audience2,
                IssuerSigningToken = issuerSigningToken2

            };
            try
            {
                var handler2 = new JwtSecurityTokenHandler();
                var newToken2 = handler2.CreateToken(new SecurityTokenDescriptor
                {
                    Token = new JwtSecurityToken(token2)
                });
                var user2 = handler2.ValidateToken(token2, validationParameters2, out var validatedToken2);
                Console.WriteLine($"Token2 {validatedToken2.Id} is validated. User Id {user2?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while validating token2: {e.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }

        

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters parameters)
        {

            return true;
        }

        private static bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters parameters)
        {
            return true;
        }

        private static string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters parameters)
        {
            return "";
        }

        private static void IssuerSigningKeyValidator(SecurityKey securityKey)
        {
            return;
        }

        private static SecurityKey IssuerSigningKeyResolver(string s, SecurityToken securityToken, SecurityKeyIdentifier keyIdentifier, TokenValidationParameters parameters)
        {
            var key = parameters?.IssuerSigningToken?.SecurityKeys?.FirstOrDefault();
            return key;
        }
    }
}
//CertificateValidator = X509CertificateValidator.None,
//ValidateIssuer = false,
//RequireSignedTokens = false,
//ValidateAudience = false,
//ValidateLifetime = false,
//ValidateIssuerSigningKey = false,
//ValidateActor = false,
//RequireExpirationTime = false,
//        IssuerSigningKeyResolver = IssuerSigningKeyResolver,
//     IssuerSigningKey = configuration?.SigningTokens?.FirstOrDefault()?.SecurityKeys?.FirstOrDefault(),
//          IssuerSigningKeys = configuration.JsonWebKeySet.Keys as IEnumerable<SecurityKey>,
//         IssuerSigningTokens = configuration.JsonWebKeySet.GetSigningTokens(),
//    IssuerSigningKeyResolver = 
