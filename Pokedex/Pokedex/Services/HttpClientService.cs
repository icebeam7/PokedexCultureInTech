using System;
using System.Net.Http;

namespace Pokedex.Services
{
    public class HttpClientService
    {
        public static HttpClient CreateClient(string baseUrl)
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
    }
}
