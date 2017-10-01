using System;
using System.Configuration;
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
            const string token = "eyJ0eXAiOiJKV1QiLCJraWQiOiJDanhYcUhFYlpvWEdYNkRHTE4xZzRaY1hCWTQ9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJURVNUMTAiLCJhdWRpdFRyYWNraW5nSWQiOiJkZWJlYTM5NC0zOTM4LTRiZjAtODk4NS05MTNjYTE5MDFiY2UiLCJpc3MiOiJodHRwczovL3d3dy50ZXN0LnZlZ3Zlc2VuLm5vOjQ0My9vcGVuYW0vb2F1dGgyL2VtcGxveWVlcyIsInRva2VuTmFtZSI6ImFjY2Vzc190b2tlbiIsInRva2VuX3R5cGUiOiJCZWFyZXIiLCJhdXRoR3JhbnRJZCI6IjdkZmRhMWQ4LTQzZmItNDdlYi05MDA4LTdlYmRmZjNlYzQzYiIsImF1ZCI6IkNvbUFyb3VuZCIsIm5iZiI6MTUwNjg3MTE2MCwic2NvcGUiOlsic3Z2cHJvZmlsZSIsIm9wZW5pZCJdLCJhdXRoX3RpbWUiOjE1MDY4NzExNjAsInJlYWxtIjoiL0VtcGxveWVlcyIsImV4cCI6MTUwNjg5OTk2MCwiaWF0IjoxNTA2ODcxMTYwLCJleHBpcmVzX2luIjoyODgwMDAwMCwianRpIjoiMDE2NmJlMTktNWUwNy00MzA3LWE5ZGUtMjExMzgzNmJkZjg2In0.AvRweqzQWQMFLoTs-39QSHJ6iUVBQqEXOr82LFsMgtOItQbwPQtq1Pl8ZNsk3TX4fIa2wMH0Op7vXNa79FsJnZCK51zrbTyCAis0r9Cg_e9RFhbQNfqh9gxiLfbiiG2X74cix97qCXh9ETrwSrbEHsbDnaI6HZ2ImaA6PP5w8qY7D3Xs8vn6Vh28r_7Z77B8GxzJFnXm_VNoXymiSLXUjdMVJ2qovU2bxBP6J48QfFzxGgUr0hTuedGM-mBwy7vSbuMThNpIyvo1ICt3ji4tWwr83ML8k2g3l9ezFkp-c5VdDHMEaDCk3-j9vRp3J_4-9Q9iDsGXFnd2gvfBInKJuQ";
            //const string domain = "https://testlogin.comaround.com/";
            //const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyIsImtpZCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyJ9.eyJpc3MiOiJodHRwczovL3Rlc3Rsb2dpbi5jb21hcm91bmQuY29tLyIsImF1ZCI6Imh0dHBzOi8vdGVzdGxvZ2luLmNvbWFyb3VuZC5jb20vcmVzb3VyY2VzIiwiZXhwIjoxNTA2ODc0NTUyLCJuYmYiOjE1MDY4NzA5NTIsImNsaWVudF9pZCI6Inplcm9mcm9udGVuZCIsInNjb3BlIjoiemVyb2FwaSIsInN1YiI6IjYxOTAiLCJhdXRoX3RpbWUiOjE1MDY4NzA4OTgsImlkcCI6Imlkc3J2IiwibmFtZSI6Ik1hcnRpbiBXw6VnZXIiLCJhbGxvd2VkaXAiOiIqIiwiY3VzdG9tZXJpZCI6IjEzOSIsImVtYWlsIjoibWFydGluLndAY29tYXJvdW5kLnNlIiwidXNlcnVnYW0iOiIxNjQxNyIsImxpY2Vuc2UiOiJCYXNpYyIsInBvcnRhbGlkcyI6Ijk3OCIsInJvbGUiOiJNYW5hZ2VyIiwidXNlcmdyb3VwaWRzIjpbIjE0MjUiLCIxNDQ1IiwiMzkwNyJdLCJhbXIiOlsiUGFzc3dvcmQiXX0.iXqo_cIz9NQMr9cSDwq87M1o5Ot5PfQoL8ewnyLAYAoI-dXpA5S0IfoB5KXZz6OPEyZRtfSCqNAYn73HhSDTNaMR2AUG01JPh7KiCph38fawEvGJvsXSRmcrSTEkQrhMI8z_caVz3clAMUrZh19WB_cZrv4PlDAeW37pf-d1dvQNORlw59tjohqWCzB-47FAn2uKVQiO379AdqP6xqa4ZJ-9KTrUMl8BNyUG8HkpPWgBagJmctiLYllpRtq_kgtuqmSYbM5g1-rmx6CrXshZo3fbXepXgBSczoGhNGMcNcucLJwqZgneuGIsBg9a30FdKoMMCz2JYq1lERq7mz6PGA";
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
                var can = handler.CanValidateToken;
                var user = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                Console.WriteLine($"Token is validated. User Id {user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
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
