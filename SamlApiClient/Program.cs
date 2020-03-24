using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HelseId.Samples.SamlApiClient
{
    class Program
    {
        private const string RsaXml = "<RSAKeyValue><Modulus>a1v3lwzMuCBKyrqjkrYS444qoovenHcaV5w0yM8NMEnUCkr3UqQ0UDkgfLt1LARUTzn2/O5VKtX6nDP63q3yO7/HMY8C9HmjJG03YmSiFRpRXL2MdDKdlL+VDwcctLEs2+5+6gWUG36uKM1XyBybumefyua1Uba4guQIS8uaJXIeFbYYzqmbMD41yio682m4Yek82hy4W5qarAA0pe3GminchUQVS4hF3vI3K7s+QBUFsneiSkyYucElWs/RgYyLVqJSGTncR48Cs20LmBHZ9CKMzzK+3YZ3sGXtJg+6ttTctUiJW3+68LDqJcSqU2P78pcAp1tD1cGUOUGG6rfc/YXUAIiDQ5r9nrscaaQBBpqNlP8PNb9b+IFLT2zEwCMp4aB0llJ4lp7JCdxUPJ4WVuRWOCeRgSqfR9l51Veq802mXrmaYyvGICBnxJJrRX06NeoIftmeRYPcOnpmRBOEhl/NXQMvCmvQPapCXHpDjuDuFF1STyBqPaPjsamrGeWXUkBhhngA+uA3FXEf3ATaaMgEED1xead+n//h0CXFwBdaBkhc31h0hl2BMdIN/Zov+8O6lNyhOwLqFCHzw0Gp2pWPEskv52Qv32/d08GgCsuYH34CyIrBp7TjJ1CnFYivMPNVoPo8/KgmkNdFs2ILHKa6iq7HrPMQ28Pmx/pCXKU=</Modulus><Exponent>AQAB</Exponent><P>0pJuMuivWzvMUROWT0lDj1H2oGF9eZSpRaTGUL8Z1q3K17PmGy/+j3WMRm81XRJr+9qidPBIXhmk7LnPmN5/nl2fLMHrfm59mWXZHL5W9ncE46BkYz0iRk4vFUbNRcPDA5OIlxbWrPc+TA1rzzCPMxo80qInur9Nq1khSd2W/zG6Etx+Kp9q6Py0ylFRkP+Fc+RU11iWnFzDiA4eC0iq67IeJF/AOoc/ir1kAIhkn/qhUczK5t6ebu2N+MyVEFKSMhzZBSver6Uku08YqaDWA5j/iMPP9Aw8L40coNwxGMtjUx5MpyFhmWFdV2m8F8eCMzw514Mty1FVUpUAqPX2gw==</P><Q>y3b2cUbeQCeHkL+U1CCgPkbxU56EQLdN/y8Izq9aLLBf6OcJsfnDANCc3OMweRD20alUmxIX41gQTYJ0K9pgM7lRTuPRcWJW89xKnCsc+Bg4+hDghbLqFFgg+/6ptwfDaCTdPD+WsYQRSJe/JjiMFbe5XU+hSa7ZbspO1k869acJOO9kB/oDZnHwEAx28BkRBR7LCP/h8h6uMbte0yiBmnl5ovfyBfyICCU642WPOra9K2wmtCdaloWkmRBIyiEYao7x0AroePnqDLTOXYd4GoavqZVq50+BrRgua5+arzAqvrCWykO2kUDP3JQujFLIo5y/qOTtRI2z737DelM3tw==</Q><DP>qh+L0K2VHwyM4eQFSEFUx/HcY27gRN4KdC3P22TJp1v5yZOakNSRwa2iizVF09ASVgQpxHhsvznQuUDVrBf22yegdjSl4hu6dbiHVGWjNLSryovHDzZQ/qQj/fiZ14d1guorLIZTIqMOPbuKInaE+zBze2lu172/LnRwJJFWcQ7n2l0xwZXSdjHUjrBsSc1nMF6E/Qahh+qaPs3JECzBinL5T0HcuGyUta6VoKiRQ37l3oSqWSP6tHxQe3Yt6GYNn1cXLspmu1mc94fL0SAUSAvQR9qLpAxOg8xqGLxNHk8UDA8qtsyNYbH8C6dtQ3j4hBRgVvGwiddIK9QeGGO/qQ==</DP><DQ>a3id+f2d/bMjl2Cqw1WsbtjYNfwADZMFXupAM7RJ5FsRfhszcs/jofWPNdnHS9ubE+nmZ7ap6YslqVtj85n4wLl9ajdJ9SMlnM/alRzsw1tAFU5+2gBERpS6b4D3slcmb0cxmNZZydBhtL961zx9Oid+gPxDzIDQFwZDmE3nbcRaSbmhU9lKnH1IeaGr3WzQIa0/P7Sxa0urZVd8Yfr+YlMR3fQr4d+fFvZbYavOeQv3Zg1NcFFtNx7Gb5c7a5EJrZdtwR9R5jzT1PxYGO0qkpBcDy5+dkn3zC9+rZhzg1/k5C6wp4wWzii24uNepv4/PrTYQ+UQMurKhZGmvWFhhw==</DQ><InverseQ>xJHmtrcMnMu5Vg0FiFWfgcivU4wIANjdNbbdaBWJU/KLnHzctWnBaB9mZlydzCWYRGmmhzwAm9X0OEb1gpT6eiSgg01/pzeIeevhBkFquw3fIRgJK2nxfiDxsSQAfjHfCQ3fEfTHSXQDd+jExbw5w681VBx+45FtU69VvJmadWQ0kCJsQVMt3e11Al07gmILlU1bjvpGeb1KZ1NiCR1O75aUgdCHyRwLwSZly2XzliO7prbnc7URnMsu+HxuQ6JybSPEWX14Pcn1BH6jJkmqflKLSKT/qiISCxD/GP+L7Jeda3fAUtW2aevFKneIk1hARa842mW/H8MQJUK6BrOTmA==</InverseQ><D>YMiiqkvQqDqkhhDhP5rj2Y0Bwva4Sivmo/vF2stCiUZoxXsNBFHJnwsqanfODyKBzz9qQmNiBV+xilvVHKnjiAIkI9jckJ03Z31xpgkkYqfRnZxQeXI8ByW0AfjO9P/xPU7zPkrzl+LuvNHjjepddLMwiZpaCWNt2OQemBaqkjUoiM3CEuGqyX9wg/VgGhxtcNH9SvWI+BC0mfuUdtDHJahHyxnQZtnr7j6NAVFLcqu1m7vrsqQRPnsgKyA7vHuWqQc+CzCW3xspKLJLHipUrQa9/6UNE/cLiIupVXWLOOhoqr3EEZIQfdkRz72n8onDzkrdKetxk1Bbc7EdYOfreBJX7Udh0cJ7bAW0gdzDinCUnu3iYOb8lnptcVlIKsXIfX1+1PoK2NOHhIX5NkhzjA0WkmYZc6LlpAFLDcC5FeNC0zIpJWlPNeFmxVqD5LhozwHz/6HcX1ohv+oD+hbF7d8tsLWOw2ROVVFSkqyN2IOC30DGrUfJxQ7si/oDeX8T9IAledZZR8UC5XmJytdLY2Ak3tw39ZaNbFlFAAQkOSKgg29Bmw5ZPzzV74qeJIUFp9Z6afsq2kLGWTOGyiuh/qQkbBv/WlCYMtKfCHPdgtOe7vISn8lrWoyfWZ5imkhCmlNqJHnawFyGD0kB3mlZtzxM6jSzOB73VRGXAoVznRE=</D></RSAKeyValue>";
        private const string ClientId = "saml-api-test";

        static async Task Main(string[] args)
        {
            var accessToken = 
                await LogonAndGetAccessToken();
            var signedJwt = GetSignedJwt();
            var samlToken = await GetSamlToken(accessToken, signedJwt);

            Console.WriteLine(samlToken.IndentXml());
        }

        private static async Task<string> GetSamlToken(string accessToken, string signedJwt)
        {
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            var response = await client.PostAsync(@"https://helseid-xdssaml.test.nhn.no/Token", new StringContent(signedJwt));
            return await response.Content.ReadAsStringAsync();
        }

        private static string GetSignedJwt()
        {
            var claims = new Dictionary<string, string>
            {
                { "subject:organization", "Onairda Sykehuspartner" },
                { "subject:organization-id", "111292000"},
                { "subject:child-organization", "childorg"},
                { "subject:child-organization_authority", "childorgauth"},
                { "subject:child-organization_codeSystem", "childorgcodesystem"},
                { "subject:facility", "facility"},
                { "subject:facility_authority", "facilityauth"},
                { "subject:facility_codeSystem", "facilitycodesystem"},
                { "subject:role:codeSystem", "http://hl7.org/fhir/ValueSet/participant-role" },
                { "subject:role:codeSystemName", "Participant Roles" },
                { "subject:role:code", "224608005" },
                { "subject:role:displayName", "Administrative healthcare staff" },
                { "homeCommunityId", "2.16.578.1.12.4.1.7.x.x"},
                { "subject:purposeofUse", "1"},
                { "resource:resource-id", "13116900216"},
                { "client_id", ClientId }
            };

            return JwtGenerator.Generate(ClientId, "norsk-helsenett:samlapi", claims, TimeSpan.FromMinutes(10), JwtGenerator.SigningMethod.RsaSecurityKey, GetSecurityKey(), SecurityAlgorithms.RsaSha512);
        }

        private static RsaSecurityKey GetSecurityKey()
        {
            var rsa = new RSACng();
            rsa.FromXmlString(RsaXml);
            var securityKey = new RsaSecurityKey(rsa);
            return securityKey;
        }

        private static async Task<string> LogonAndGetAccessToken()
        {
            var options = new OidcClientOptions
            {
                Authority = "https://helseid-sts.test.nhn.no",
                ClientId = ClientId,
                //ClientSecret = "N1OQIUAeqXYwuAjZwoq91VoLrxj28vlMggZtAfWzgZXQiQoEyVq33jSqiMF9IecC",
                Scope = "openid profile helseid://scopes/identity/pid helseid://scopes/identity/pid_pseudonym helseid://scopes/identity/security_level helseid://scopes/hpr/hpr_number norsk-helsenett:samlapi/xds offline_access",
                RedirectUri = "http://localhost:6501/signin-oidc",
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.FormPost
            };

            var codeParameters = GetClientAssertionParameters("https://helseid-sts.test.nhn.no");

            var client = new OidcClient(options);

            var state = await client.PrepareLoginAsync(codeParameters);

            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:6501/");
            httpListener.Start();

            var url = state.StartUrl.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });

            var context = await httpListener.GetContextAsync();
            var response = await context.Request.AsStringAsync();

            var accessTokenParameters = GetClientAssertionParameters("https://helseid-sts.test.nhn.no/connect/token");

            var result = await client.ProcessResponseAsync(response, state, accessTokenParameters);

            var responseText = Encoding.UTF8.GetBytes("You are now logged on. You can close this tab.");
            await context.Response.OutputStream.WriteAsync(responseText);
            context.Response.StatusCode = 200;
            context.Response.Close();

            return result.AccessToken;
        }

        private static Dictionary<string, string> GetClientAssertionParameters(string audience)
        {
            var clientAssertion = JwtGenerator.Generate(ClientId, audience, JwtGenerator.SigningMethod.RsaSecurityKey, GetSecurityKey(), SecurityAlgorithms.RsaSha512);

            var clientAssertionParameters = new Dictionary<string, string>();
            clientAssertionParameters.Add("client_assertion", clientAssertion);
            clientAssertionParameters.Add("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            return clientAssertionParameters;
        }
    }
}
