using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.Domain.Entities;

namespace WEB_253502_HVESKO.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполнить миграции, если их еще не было
            await context.Database.MigrateAsync();

            // Получение базового адреса из appsettings.json
            var config = app.Configuration;
            string baseUrl = config["UriData:ApiUri"];

            // Проверка, есть ли уже данные в базе
            if (!await context.Categories.AnyAsync())
            {
                // Добавляем категории
                var categories = new List<Category>
                {
                    new Category { Name = "Лазерная косметология", NormalizedName = "laser-cosmetology" },
                    new Category { Name = "Ультразвуковая кавитация", NormalizedName = "ultrasound-cavitation" },
                    new Category { Name = "Дермабразия", NormalizedName = "dermabrasion" },
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Проверка, есть ли уже данные в базе услуг
            if (!await context.Products.AnyAsync())
            {
                // Получаем категории из базы данных
                var categories = await context.Categories.ToListAsync();

                // Добавление услуг
                var services = new List<Service>
                {
                    new()
                    {
                        Name = "Лазерное омоложение",
                        Description = "Лазерное омоложение - это современная косметологическая процедура...",
                        Price = 80,
                        ImagePath = $"{baseUrl}/Images/laser1.jpg",
                        Category = categories.Find(c => c.NormalizedName.Equals("laser-cosmetology"))
                    },
                    new()
                    {
                        Name = "Лазерная шлифовка",
                        Description = "Метод фракционного омоложения основан на воздействии тепловой энергии...",
                        Price = 40,
                        ImagePath = $"{baseUrl}/Images/laser2.jpg",
                        Category = categories.Find(c => c.NormalizedName.Equals("laser-cosmetology"))
                    },
                    new()
                    {
                        Name = "Ультразвуковая чистка лица",
                        Description = "Ультразвуковая чистка лица – это безболезненная неинвазивная аппаратная процедура...",
                        Price = 95,
                        ImagePath = $"{baseUrl}/Images/ultrasound1.jpg",
                        Category = categories.Find(c => c.NormalizedName.Equals("ultrasound-cavitation"))
                    },
                    new()
                    {
                        Name = "Подъем бровей",
                        Description = "Пластический подъем бровей, также известный как броулифтинг...",
                        Price = 1900,
                        ImagePath = $"{baseUrl}/Images/dermabrasion1.jpg",
                        Category = categories.Find(c => c.NormalizedName.Equals("dermabrasion"))
                    },
                    new()
                    {
                        Name = "Ринопластика: коррекция кончика носа",
                        Description = "Ринопластика кончика носа - это подвид пластики носа...",
                        Price = 2000,
                        ImagePath = $"{baseUrl}/Images/dermabrasion2.jpg",
                        Category = categories.Find(c => c.NormalizedName.Equals("dermabrasion"))
                    },
                };

                await context.Products.AddRangeAsync(services);
                await context.SaveChangesAsync();
            }
        }
    }
}
