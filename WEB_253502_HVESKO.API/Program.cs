using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.API.Models;
using WEB_253502_HVESKO.API.Services.CategoryService;
using WEB_253502_HVESKO.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Чтение строки подключения из файла appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Default");

// Регистрация контекста базы данных с использованием строки подключения
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var authServer = builder.Configuration.GetSection("AuthServer").Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        // Адрес метаданных конфигурации OpenID
        o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

        // Authority сервера аутентификации
        o.Authority = $"{authServer.Host}/realms/{authServer.Realm}"; ;
        // Audience для токена JWT
        o.Audience = "account";
        // Запретить HTTPS для использования локальной версии Keycloak
        // В рабочем проекте должно быть true
        o.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Подключаем поддержку статических файлов
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

DbInitializer.SeedData(app);

app.MapControllers();

app.Run();
