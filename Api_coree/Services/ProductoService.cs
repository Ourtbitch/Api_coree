using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Api_coree.Models;
using Newtonsoft.Json;

public class ProductoService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public ProductoService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    // Obtener lista de productos
    public async Task<List<Producto>> GetProductosAsync()
    {
        var token = await _authService.AuthenticateAsync("m@gmail.com", "123");
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("No se pudo autenticar el usuario.");
            return new List<Producto>();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync("http://localhost:5243/api/Producto/lista");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Error al obtener los datos de la API. Código de estado: " + response.StatusCode);
            return new List<Producto>();
        }

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Producto>>>(content);

        // Verifica si la respuesta contiene productos
        return apiResponse?.Response ?? new List<Producto>();
    }


    // Obtener un producto por ID
    public async Task<Producto> GetProductoAsync(int idProducto)
    {
        string token = await _authService.AuthenticateAsync("m@gmail.com", "123");
        if (string.IsNullOrEmpty(token)) throw new System.Exception("No se pudo autenticar el usuario.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"http://localhost:5243/api/Producto/Obtener/{idProducto}");

        if (!response.IsSuccessStatusCode)
        {
            throw new System.Exception($"Error en la solicitud: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Contenido de la respuesta de la API para GetProductoAsync: " + content);

        var producto = JsonConvert.DeserializeObject<Producto>(content);

        // Agrega un punto de depuración o imprime el objeto para verificarlo
        Console.WriteLine("Producto obtenido: " + JsonConvert.SerializeObject(producto));

        return producto;
    }



    // Guardar un nuevo producto
    public async Task<bool> GuardarProductoAsync(Producto producto)
    {
        string token = await _authService.AuthenticateAsync("m@gmail.com", "123");
        if (string.IsNullOrEmpty(token)) throw new System.Exception("No se pudo autenticar el usuario.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync("http://localhost:5243/api/Producto/Guardar", producto);
        return response.IsSuccessStatusCode;
    }

    // Editar un producto existente
    // Editar un producto existente
    public async Task<bool> EditarProductoAsync(Producto producto)
    {
        bool respuesta = false;

        // Autenticación
        string token = await _authService.AuthenticateAsync("m@gmail.com", "123");
        if (string.IsNullOrEmpty(token))
        {
            throw new System.Exception("No se pudo autenticar el usuario.");
        }

        // Crear un nuevo cliente HTTP y configurar la base de la dirección
        var cliente = new HttpClient();
        cliente.BaseAddress = new Uri("http://localhost:5243");

        // Añadir el token de autenticación a las cabeceras de la solicitud
        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Serializar el objeto Producto en JSON
        var content = new StringContent(JsonConvert.SerializeObject(producto), Encoding.UTF8, "application/json");

        // Realizar la solicitud PUT a la API para editar el producto
        var response = await cliente.PutAsync($"/api/Producto/Editar/{producto.IdProducto}", content);

        // Verificar si la respuesta fue exitosa
        if (response.IsSuccessStatusCode)
        {
            respuesta = true;
        }

        return respuesta;
    }



    // Eliminar un producto por ID
    public async Task<bool> EliminarProductoAsync(int idProducto)
    {
        string token = await _authService.AuthenticateAsync("m@gmail.com", "123");
        if (string.IsNullOrEmpty(token)) throw new System.Exception("No se pudo autenticar el usuario.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"http://localhost:5243/api/Producto/Eliminar/{idProducto}");
        return response.IsSuccessStatusCode;
    }

}
