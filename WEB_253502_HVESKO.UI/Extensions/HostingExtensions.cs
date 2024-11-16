using WEB_253502_HVESKO.UI.Services.CategoryService;
using WEB_253502_HVESKO.UI.Services.FileService;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
            builder.Services.AddScoped<IProductService, ApiProductService>();

            var UriData = builder.Configuration.GetSection("UriData").Get<UriData>();

            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
                opt.BaseAddress = new Uri($"{UriData.ApiUri}Files"));
        }
    }
}
