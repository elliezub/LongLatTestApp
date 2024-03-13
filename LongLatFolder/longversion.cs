using System; // from here down this code works 
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LongLatTestApp
{
    public class LongVersion
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public LongVersion(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<string> GetGeolocationAsync(string address)
        {
            var relativeUri = $"maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(relativeUri);
                response.EnsureSuccessStatusCode();
                var root = await response.Content.ReadFromJsonAsync<JsonElement>();

                if (root.GetProperty("status").GetString() == "OK")
                {
                    var location = root.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
                    double latitude = location.GetProperty("lat").GetDouble();
                    double longitude = location.GetProperty("lng").GetDouble();

                    return $"Latitude: {latitude}, Longitude: {longitude}";
                }
                else
                {
                    return "Error retrieving geolocation.";
                }
            }
            catch (Exception e)
            {
                return $"\nException Caught! Message: {e.Message}";
            }
        }
    }
}
