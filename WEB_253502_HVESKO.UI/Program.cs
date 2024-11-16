using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.UI;
using WEB_253502_HVESKO.UI.Data;
using WEB_253502_HVESKO.UI.Extensions;
using WEB_253502_HVESKO.UI.Services.CategoryService;
using WEB_253502_HVESKO.UI.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Получаем UriData из конфигурации
var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.RegisterCustomServices();

// Регистрируем сервисы для запросов к API
builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
{
    opt.BaseAddress = new Uri(uriData.ApiUri);
});

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
{
    opt.BaseAddress = new Uri(uriData.ApiUri);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
