using BlazorHybrid.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class RegistrationService
{
    private readonly HttpClient _httpClient;

    public RegistrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RegisterUser(User user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44384/api/User/Registration", user);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
