using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Shop.Jobs.PaymentConciliation
{
    class Program
    {
        private const string IDENTITY_ENDPOINT = "https://localhost:1001";
        private const string PRODUCTS_API_ENDPOINT = "https://localhost:2001";

        static async Task Main(string[] args)
        {
            await ConnectAsync();
        }

        public static async Task ConnectAsync()
        {
            var client = new HttpClient();

            // Discovery the services
            var disco = await client.GetDiscoveryDocumentAsync(IDENTITY_ENDPOINT);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "Shop.Jobs.PaymentConciliation", // Definido no Config.cs do Shop.Identity
                ClientSecret = "secret",// Definido no Config.cs do Shop.Identity
                Scope = "Shop.Api.Products" // Definido no Config.cs do Shop.Identity
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync($"{PRODUCTS_API_ENDPOINT}/products");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
