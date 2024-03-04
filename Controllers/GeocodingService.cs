using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeocodingService(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<(double Latitude, double Longitude, string? City)> GetCoordinatesAndCityAsync(string address)
        {
            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
            

            var response = await _httpClient.GetFromJsonAsync<GeocodingResponse>(apiUrl);

            if (response?.Results?.Count > 0)
            {
                var location = response.Results[0].Geometry.Location;

                string key = Config.Config.BingMapsApiKey;
                string apiAddress = $"https://dev.virtualearth.net/REST/v1/Locations/{location.Lat},{location.Lng}?o=json&key={key}";
                string? adminDistrict2 = null;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseBing = await client.GetAsync(apiUrl);

                    if (responseBing.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string jsonResponse = await responseBing.Content.ReadAsStringAsync();

                        if (jsonResponse != null)
                        {
                            // Parse the JSON response using JsonDocument
                            JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);

                            adminDistrict2 = jsonDocument.RootElement
                            .GetProperty("results")[0]
                            .GetProperty("address_components")
                            .EnumerateArray()
                            .FirstOrDefault(c => c.GetProperty("types").EnumerateArray().Any(t => t.GetString() == "administrative_area_level_2"))
                            .GetProperty("long_name")
                            .GetString();
                        }

                        return (location.Lat, location.Lng, adminDistrict2);
                    }
                    else
                    {
                        return (0, 0, string.Empty);
                    }
                }
            }
          

            // Handle error or empty response
            return (0, 0, string.Empty);
        }

        public async Task<(string? Departamento, string? Ciudad, string? Direccion)> GetCoordinateDetailsAsync(double Lat, double Lng)
        {
            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={Lat},{Lng}&key={_apiKey}";

            string? departamento = null;
            string? ciudad = null;
            string? direccion = null;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseBing = await client.GetAsync(apiUrl);

                    if (responseBing.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string jsonResponse = await responseBing.Content.ReadAsStringAsync();

                        if (jsonResponse != null)
                        {
                            // Parse the JSON response using JsonDocument
                            JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);

                        departamento = jsonDocument.RootElement
                            .GetProperty("results")[0]
                            .GetProperty("address_components")
                            .EnumerateArray()
                            .FirstOrDefault(c => c.GetProperty("types").EnumerateArray().Any(t => t.GetString() == "administrative_area_level_1"))
                            .GetProperty("long_name")
                            .GetString();

                        ciudad = jsonDocument.RootElement
                            .GetProperty("results")[0]
                            .GetProperty("address_components")
                            .EnumerateArray()
                            .FirstOrDefault(c => c.GetProperty("types").EnumerateArray().Any(t => t.GetString() == "administrative_area_level_2"))
                            .GetProperty("long_name")
                            .GetString();

                        direccion = jsonDocument.RootElement
                            .GetProperty("results")[0]
                            .GetProperty("formatted_address")
                            .GetString();
                    }

                        return (departamento, ciudad, direccion);
                    }
                    else
                    {
                        return (string.Empty, string.Empty, string.Empty);
                    }
                }
        }



        public string GetStaticMapImageUrl(double latitude, double longitude)
        {
            // Example URL, you may customize the parameters as needed
            return $"https://maps.googleapis.com/maps/api/staticmap?center={latitude},{longitude}&zoom=15&size=400x400&markers=color:red%7C{latitude},{longitude}&key={_apiKey}";
        }
    }
}
