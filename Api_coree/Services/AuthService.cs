using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AuthenticateAsync(string correo, string clave)
    {
        var requestData = new { correo, clave };
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5243/api/Autenticacion/validar", requestData);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content);
            return result.token;
        }

        return null;
    }
}
