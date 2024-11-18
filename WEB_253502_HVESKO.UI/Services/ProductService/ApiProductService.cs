using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;
using WEB_253502_HVESKO.UI.Services.ProductService;
using System.Text.Json;
using WEB_253502_HVESKO.UI.Services.FileService;
using WEB_253502_HVESKO.UI.Services.Authentication;

namespace WEB_253502_HVESKO.UI.Services.ProductService;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiProductService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly string _pageSize;
    private readonly IFileService _fileService;
    private readonly ITokenAccessor _tokenAccessor;

    public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _pageSize = configuration.GetSection("ItemsPerPage").Value ?? "3";
        _fileService = fileService;
        _tokenAccessor = tokenAccessor;
    }

    public async Task<ResponseData<ListModel<Service>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        // Подготовка URL
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Services");

        // Добавляем категорию в маршрут
        if (categoryNormalizedName != null)
        {
            urlString.Append($"/{categoryNormalizedName}?");
        }
        else
        {
            urlString.Append("?");
        }

        // Добавляем номер страницы в маршрут
        if (pageNo > 1)
        {
            urlString.Append($"pageNo={pageNo}");
        }
        else
        {
            urlString.Append("pageNo=1");
        }

        // Добавляем размер страницы в строку запроса
        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize));
        }

        // Отправляем запрос к API
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Service>>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return ResponseData<ListModel<Service>>.Error($"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"Данные не получены от сервера. Error: {response.StatusCode}");
        return ResponseData<ListModel<Service>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
    }

    public async Task<ResponseData<Service>> GetProductByIdAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Services/id-{id}");
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Service>>(_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return ResponseData<Service>.Error($"Ошибка: {ex.Message}");
            }
        }

        _logger.LogError($"Продукт с ID {id} не найден. Error: {response.StatusCode}");
        return ResponseData<Service>.Error($"Продукт с ID {id} не найден.");
    }
    public static string GetFileName(string path)
    {
        // Если это URL, извлекаем все после последнего '/'
        if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
        {
            Uri uri = new Uri(path);
            return Path.GetFileName(uri.LocalPath);
        }

        // Если это относительный путь, просто извлекаем имя файла
        return Path.GetFileName(path);
    }
    public async Task UpdateProductAsync(int id, Service product, IFormFile? formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        if (formFile != null)
        {
            var path = product.ImagePath;
            var filename = GetFileName(path);
            await _fileService.DeleteFileAsync(filename);
            //await _fileService.DeleteFileAsync(path);
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
                product.ImagePath = imageUrl;
        }

        var uri = new Uri($"{_httpClient.BaseAddress}Services/{id}");

        var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Не удалось обновить продукт с ID {id}. Error: {response.StatusCode}");
            throw new Exception($"Не удалось обновить продукт с ID {id}");
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.DeleteAsync($"Services/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Не удалось удалить продукт с ID {id}. Error: {response.StatusCode}");
            throw new Exception($"Не удалось удалить продукт с ID {id}");
        }
    }

    public async Task<ResponseData<Service>> CreateProductAsync(Service product, IFormFile? formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        product.ImagePath = "Images/noimage.jpg";
        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
                product.ImagePath = imageUrl;
        }
        var uri = new Uri($"{_httpClient.BaseAddress}Services");

        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Service>>(_serializerOptions);
            return data;
        }

        _logger.LogError($"Продукт не создан. Error: {response.StatusCode}");
        return ResponseData<Service>.Error($"Продукт не создан. Error: {response.StatusCode}");
    }
}
