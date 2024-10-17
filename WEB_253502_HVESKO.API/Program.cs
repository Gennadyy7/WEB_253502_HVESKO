using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;

var builder = WebApplication.CreateBuilder(args);

// ������ ������ ����������� �� ����� appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Default");

// ����������� ��������� ���� ������ � �������������� ������ �����������
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ���������� ��������� ����������� ������
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

DbInitializer.SeedData(app);

app.MapControllers();

app.Run();
