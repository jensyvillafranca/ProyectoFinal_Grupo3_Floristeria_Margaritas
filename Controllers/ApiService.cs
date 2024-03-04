using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<T> GetDataAsync<T>(string endpoint)
        {
            try
            {
                string url = $"https://phpclusters-164276-0.cloudclusters.net/{endpoint}";
                var response = await _httpClient.GetStringAsync(url);

                // Deserialize JSON response
                return System.Text.Json.JsonSerializer.Deserialize<T>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return default;
            }
        }

        public async Task<TResponse> PostDataAsync<TResponse>(string endpoint, object data)
        {
            var apiUrl = "https://phpclusters-164276-0.cloudclusters.net/";

            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{apiUrl}{endpoint}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }

                // Handle errors, you can throw an exception or return a default value
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostDataAsync: {ex.Message}");
                return default;
            }
        }

        public async Task<bool> PostSuccessAsync(string endpoint, object data)
        {
            var apiUrl = "https://phpclusters-164276-0.cloudclusters.net/";

            try
            {
                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{apiUrl}{endpoint}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostSuccessAsync: {ex.Message}");
                return false;
            }
        }
    }
}
