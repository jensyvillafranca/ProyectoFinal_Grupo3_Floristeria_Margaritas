using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                return JsonSerializer.Deserialize<T>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return default;
            }
        }
    }
}
