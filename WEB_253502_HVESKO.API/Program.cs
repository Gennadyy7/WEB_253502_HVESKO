using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.API.Services.CategoryService;
using WEB_253502_HVESKO.API.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Чтение строки подключения из файла appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Default");

// Регистрация контекста базы данных с использованием строки подключения
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

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

app.UseAuthorization();

DbInitializer.SeedData(app);

app.MapControllers();

app.Run();
