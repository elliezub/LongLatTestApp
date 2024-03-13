using Microsoft.Extensions.Configuration;
using System;

namespace LongLatTestApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            //both methods need these
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();
            var configuration = builder.Build();
            var apiKey = configuration["YOUR_API_KEY"];
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API key not found. Please make sure it's set in the environment variables.");
                return;
            }

            // nuget method stuff
            var locationService = new GoogleLocationServiceWrapper(apiKey);

            var address1 = "Fort Myers, Florida";
            var nugetWay = new NuGetWay(locationService);

            var result = nugetWay.GetLongLatNuGetWay(address1);
            Console.WriteLine(result);

            //long version stuff
            var httpClient = new HttpClient();
            var longVersion = new LongVersion(httpClient, apiKey);

            var address = "1600 Amphitheatre Parkway, Mountain View, CA";
            // Correctly await the asynchronous operation
            var geolocationResult = await longVersion.GetGeolocationAsync(address);
            Console.WriteLine(geolocationResult);
        }
    }
}

