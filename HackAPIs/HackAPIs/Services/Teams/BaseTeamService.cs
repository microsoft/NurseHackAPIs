using HackAPIs.Services.Util;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackAPIs.Services.Teams
{
    public class BaseTeamService
    {
        public static string NurseHackTeam1 = UtilConst.MSTeam1;
        public static string NurseHackTeam2 = UtilConst.MSTeam2;

        public static string TeamDomain = UtilConst.TeamDomain;

        protected enum HttpMethodType
        {
            Get,
            Post,
            Put,
            Patch,
            Delete
        }
        protected static async Task<JObject> RunAsync(string urlExt, HttpMethodType httpMethodType, StringContent dataContent)
        {
            JObject json = null;
            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.json");


            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();

            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { $"{config.ApiUrl}.default" };

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired");
                Console.ResetColor();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Scope provided is not supported");
                Console.ResetColor();
            }

            if (result != null)
            {
                var httpClient = new HttpClient();
                var apiCaller = new TeamsApiCallHelper(httpClient);
               

                switch (httpMethodType)
                {
                    case HttpMethodType.Get:
                        {
                            json = await apiCaller.GetWebApiAndProcessResultASync($"{config.ApiUrl}" + urlExt, result.AccessToken, Display);
                            break;
                        }
                    case HttpMethodType.Post:
                        {
                            json = await apiCaller.PostWebApiAndProcessResultASync($"{config.ApiUrl}" + urlExt, result.AccessToken, Display, dataContent);
                            break;
                        }
                    case HttpMethodType.Put:
                        {
                            json = await apiCaller.GetWebApiAndProcessResultASync($"{config.ApiUrl}" + urlExt, result.AccessToken, Display);
                            break;
                        }
                    case HttpMethodType.Patch:
                        {
                            json = await apiCaller.PatchWebApiAndProcessResultASync($"{config.ApiUrl}" + urlExt, result.AccessToken, Display, dataContent);
                            break;
                        }
                    case HttpMethodType.Delete:
                        {
                            json = await apiCaller.DeleteWebApiAndProcessResultASync($"{config.ApiUrl}" + urlExt, result.AccessToken, Display);
                            break;
                        }

                }
            
            }
            return json;
        }

        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to display</param>
        private static void Display(JObject result)
        {
            foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))
            {
                Console.WriteLine($"{child.Name} = {child.Value}");
            }
        }
    }
}
