using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Local.IdentityModel.Extensions.Configuration;
using Local.IdentityModel.Tokens;

namespace tokenvalidator
{
    internal static class Program
    {
        private static void Main()
        {
            const string domain = "https://www.test.vegvesen.no:443/openam/oauth2/employees";
            const string audience = "ComAround";
            const string token = "eyJ0eXAiOiJKV1QiLCJraWQiOiJDanhYcUhFYlpvWEdYNkRHTE4xZzRaY1hCWTQ9IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJURVNUMTAiLCJhdWRpdFRyYWNraW5nSWQiOiIzZDJkNTVlZS1hNmEzLTRmNDgtYjM3MC0yZDkwNmM2MzA4NGQiLCJpc3MiOiJodHRwczovL3d3dy50ZXN0LnZlZ3Zlc2VuLm5vOjQ0My9vcGVuYW0vb2F1dGgyL2VtcGxveWVlcyIsInRva2VuTmFtZSI6ImFjY2Vzc190b2tlbiIsInRva2VuX3R5cGUiOiJCZWFyZXIiLCJhdXRoR3JhbnRJZCI6IjFkN2M0OWQ0LWM4OTItNDYxZS1iMGQ2LWIzZDNjMDk4ZDZiZSIsImF1ZCI6IkNvbUFyb3VuZCIsIm5iZiI6MTUwODAyMjM0Niwic2NvcGUiOlsic3Z2cHJvZmlsZSIsIm9wZW5pZCJdLCJhdXRoX3RpbWUiOjE1MDgwMjIzNDYsInJlYWxtIjoiL0VtcGxveWVlcyIsImV4cCI6MTUwODA1MTE0NiwiaWF0IjoxNTA4MDIyMzQ2LCJleHBpcmVzX2luIjoyODgwMDAwMCwianRpIjoiNzE3Yzk0YTYtNDdmOS00MzlhLWEwNzQtMjI4MjYyMTBiMzc5In0.kY2T0pAWopjsOmajjscXeI8HgrrQ8-e6LZprn9_LgSPRfYq5crUMxJ70rLawEMnpIkv7KEvNO3yfJNYTbif3miztbY5ipEN9CHE-2UBZqTv_A5i2-lvq52UDo4D10BL45j3ULW4VpqI_hu_JWCRxJrE5lt4AoLRywWJvdIHuaQ49krzANb2pzvZT0mwVwB0XfiwmwHCZ8uHAhVaxx9JtdiDyhfpLBstqjUpp1tfpJRpzjoqCcvKeUM7FmtnUnG6QMTTerixqBSveKEjL2naKnF7h7m4p6WjsA5POYK3Dpra12SPQ-iflr_8MbcIyVxMtj_yhCdUWAglHOPoxb0RG7Q";
            //var rsa = new RSACryptoServiceProvider();
            //rsa.ImportParameters(new RSAParameters
            //{
            //    Modulus = FromBase64Url("3-f4dQItaX04RceIkb62Xy8Xti17FyeYOdUEZYuoMBOOjPN1mWwaZhKQzsjN-YpkNJ4FckhkwOVZg0jHXwoPt4pz98Z95MoXLRp4VtnF0mL5fjrvqqN4x_SGRID84aEjK8yOSxhlweHEAq7WvgxVVGVrrviAXILGWe5HxwtXherJmwbdC-NxIrvDftlVGRU0Mg0fsQ_CcMLgJk97zU-hlZyRrUo1VvNNCM2pUMUCctgilh3cGEagg7mjge3K6BWAIkR8dm1j9smk_WFPgeg9R_yo3ufAlyHfgOGrwEnjJ27LsDvSqWVxhyHKxpS9xg8mSE6L1sUgsrnU21IudNwfhw"),
            //    Exponent = FromBase64Url("AQAB")
            //});
            //var key = new RsaSecurityKey(rsa);
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain}/.well-known/openid-configuration");
            var configuration = AsyncHelper.RunSync(async () => await configurationManager.GetConfigurationAsync(CancellationToken.None));
         //   var issuerSigningToken = configuration.SigningTokens.FirstOrDefault(x => x is NamedKeySecurityToken);
         //   var issuerSigningToken2 = new NamedKeySecurityToken("kid", "CjxXqHEbZoXGX6DGLN1g4ZcXBY4==", key);
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = domain,
                ValidAudience = audience,
           //     IssuerSigningToken = issuerSigningToken2,
                IssuerSigningTokens = configuration.SigningTokens,
                //IssuerSigningKeyValidator = IssuerSigningKeyValidator,
                //IssuerValidator = IssuerValidator,
                //AudienceValidator = AudienceValidator,
                //LifetimeValidator = LifetimeValidator,
               // IssuerSigningKeyResolver = IssuerSigningKeyResolver
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

        //    const string domain2 = "https://testlogin.comaround.com";
        //    const string token2 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyIsImtpZCI6IjZncnh0VXdnZ3FEUFFKbm1tRDRtNURpU2N6dyJ9.eyJpc3MiOiJodHRwczovL3Rlc3Rsb2dpbi5jb21hcm91bmQuY29tLyIsImF1ZCI6Imh0dHBzOi8vdGVzdGxvZ2luLmNvbWFyb3VuZC5jb20vcmVzb3VyY2VzIiwiZXhwIjoxNTA3OTk5OTUzLCJuYmYiOjE1MDc5OTYzNTMsImNsaWVudF9pZCI6Inplcm9mcm9udGVuZCIsInNjb3BlIjoiemVyb2FwaSIsInN1YiI6IjYxOTAiLCJhdXRoX3RpbWUiOjE1MDc5OTYzNTAsImlkcCI6Imlkc3J2IiwibmFtZSI6Ik1hcnRpbiBXw6VnZXIiLCJhbGxvd2VkaXAiOiIqIiwiY3VzdG9tZXJpZCI6IjEzOSIsImVtYWlsIjoibWFydGluLndAY29tYXJvdW5kLnNlIiwidXNlcnVnYW0iOiIxNjQxNyIsImxpY2Vuc2UiOiJCYXNpYyIsInBvcnRhbGlkcyI6WyIxOTciLCIyMDEiLCI5NzgiXSwicm9sZSI6Ik1hbmFnZXIiLCJ1c2VyZ3JvdXBpZHMiOlsiMTQyNSIsIjE0NDUiLCIzOTA3Il0sImFtciI6WyJQYXNzd29yZCJdfQ.T0_HfwOI70MjdL0aishYxhMwXgE9gadb8YweRcncxiryhNReaY_CODpfK0kFY5rsvEx-TSHAw4-axJxkrJzndedUVUoQFuOTpUmZFl7_tOEI1GETiKycwlwieZumSYvqdGCCKWtySnCsrF9fROOdXiIzS4K9lybx1EADql1NGx0PW1jNCfTQiHVFd3AqfCFLK_78eYBuU_JHdZIUiHHaxRJJea4VmbPOfB98lvY4gf9yaXHg8LcZ2hJZIZ9zC1PcujGqKcdpzvJblx2YVjWvnnqTyt11ctvClKlhsOCFphkn4CdA9x3mj-Q6FTwjePLRKDGL1nvmVPSXMHDgmqk0xQ";
        //    const string audience2 = "https://testlogin.comaround.com/resources";

        //    var rsa2 = new RSACryptoServiceProvider();
        //    rsa.ImportParameters(new RSAParameters
        //    {
        //        Modulus = FromBase64Url("3-f4dQItaX04RceIkb62Xy8Xti17FyeYOdUEZYuoMBOOjPN1mWwaZhKQzsjN-YpkNJ4FckhkwOVZg0jHXwoPt4pz98Z95MoXLRp4VtnF0mL5fjrvqqN4x_SGRID84aEjK8yOSxhlweHEAq7WvgxVVGVrrviAXILGWe5HxwtXherJmwbdC-NxIrvDftlVGRU0Mg0fsQ_CcMLgJk97zU-hlZyRrUo1VvNNCM2pUMUCctgilh3cGEagg7mjge3K6BWAIkR8dm1j9smk_WFPgeg9R_yo3ufAlyHfgOGrwEnjJ27LsDvSqWVxhyHKxpS9xg8mSE6L1sUgsrnU21IudNwfhw"),
        //        Exponent = FromBase64Url("AQAB")
        //    });
        //    var key2 = new RsaSecurityKey(rsa);
        ////    var configurationManager2 = new ConfigurationManager<OpenIdConnectConfiguration>($"{domain2}/.well-known/openid-configuration");
        ////    var configuration2 = AsyncHelper.RunSync(async () => await configurationManager2.GetConfigurationAsync(CancellationToken.None));
        ////    var issuerSigningToken3 = configuration2.SigningTokens.FirstOrDefault(x => x is NamedKeySecurityToken);
        //    var issuerSigningToken4 = new NamedKeySecurityToken("kid", "6grxtUwggqDPQJnmmD4m5DiSczw", key2);
            
        //    var validationParameters2 = new TokenValidationParameters
        //    {
        //        ValidIssuer = domain2 + "/",
        //        ValidAudience = audience2,
        //        IssuerSigningToken = issuerSigningToken4

        //    };
        //    try
        //    {
        //        var handler2 = new JwtSecurityTokenHandler();
        //        var user2 = handler2.ValidateToken(token2, validationParameters2, out var validatedToken2);
        //        Console.WriteLine($"Token2 {validatedToken2.Id} is validated. User Id {user2?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value}");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error occurred while validating token2: {e.Message}");
        //    }




            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }

        //private static byte[] FromBase64Url(string base64Url)
        //{
        //    var padded = base64Url.Length % 4 == 0 ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
        //    var base64 = padded.Replace("_", "/").Replace("-", "+");
        //    return Convert.FromBase64String(base64);
        //}

        //private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters parameters)
        //{

        //    return true;
        //}

        //private static bool AudienceValidator(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters parameters)
        //{
        //    return true;
        //}

        //private static string IssuerValidator(string issuer, SecurityToken securityToken, TokenValidationParameters parameters)
        //{
        //    return "";
        //}

        //private static void IssuerSigningKeyValidator(SecurityKey securityKey)
        //{
        //    return;
        //}

        //private static SecurityKey IssuerSigningKeyResolver(string s, SecurityToken securityToken, SecurityKeyIdentifier keyIdentifier, TokenValidationParameters parameters)
        //{

        //    var key = parameters?.IssuerSigningToken?.SecurityKeys?.FirstOrDefault();
        //    return key;
        //}

      

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
