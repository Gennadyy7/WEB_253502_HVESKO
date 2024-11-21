using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI;
using WEB_253502_HVESKO.UI.Extensions;
using WEB_253502_HVESKO.UI.HelperClasses;
using WEB_253502_HVESKO.UI.Middlewares;
using WEB_253502_HVESKO.UI.Services.Authentication;
using WEB_253502_HVESKO.UI.Services.Authorization;
using WEB_253502_HVESKO.UI.Services.Cart;
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

builder.Services
            .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));

builder.Services.AddScoped<Cart, SessionCart>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var keycloakData =
        builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться к     локальному Keycloak по http
    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day));

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
