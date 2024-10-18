using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;
using WEB_253502_HVESKO.UI.Services.CategoryService;

public class ApiCategoryService : ICategoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiCategoryService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;

    public ApiCategoryService(HttpClient httpClient, ILogger<ApiCategoryService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var response = await _httpClient.GetAsync("categories");

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"Ошибка при получении данных: {response.StatusCode}");
        return ResponseData<List<Category>>.Error($"Ошибка при получении данных: {response.StatusCode}");
    }
}
