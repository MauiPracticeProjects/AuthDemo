using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static BlazorHybrid.Shared.Pages.Login;

namespace BlazorHybrid.Shared
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginUser(UserModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:44384/api/User/login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                    if (result.TryGetProperty("result", out JsonElement resultElement))
                    {
                        return resultElement.GetString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public async Task<string> GetProtectedData(string token)
        {
            try
            {
                token = token.Trim();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://localhost:44384/api/secure/protectedData");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                    if (result.TryGetProperty("message", out JsonElement messageElement))
                    {
                        return messageElement.GetString();
                    }
                }
            }
            catch(Exception ex)
            {
                return "Error fetching protected data";
            }
            return "Access denied";
        }
    }
}
