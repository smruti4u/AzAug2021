using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSALAuthentication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = await GetAccessTokenMSAL();
            Console.Read();
        }


        private async static Task<string> GetAccessTokenMSAL()
        {
            const string CLientId = "0b24823b-ad06-42a6-b962-af626ac2cad0";
            const string TenantId = "549a973a-fda2-4190-9ee8-67445857b006";
            var app = ConfidentialClientApplicationBuilder.Create(CLientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
                .WithClientSecret("K5ak~p9.lD_JMo_M41-8Tunix0_JJWdr0_")
                .Build();

            var scopes = new List<string>() { "https://graph.microsoft.com/.default" };
            var authResult = app.AcquireTokenForClient(scopes).ExecuteAsync().GetAwaiter().GetResult();
            return authResult.AccessToken;
        }
    }
}
