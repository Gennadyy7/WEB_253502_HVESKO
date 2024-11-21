using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.API.Services.ProductService;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:") // Используем SQLite in-memory
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection(); // Открываем соединение
            context.Database.EnsureCreated(); // Создаем схему базы данных
            return context;
        }

        [Fact]
        public void ServiceReturnsFirstPageOfThreeItems()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            // Добавляем категорию один раз
            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Service { Name = "Service1", Description = "Description1", Category = category },
                new Service { Name = "Service2", Description = "Description2", Category = category },
                new Service { Name = "Service3", Description = "Description3", Category = category },
                new Service { Name = "Service4", Description = "Description4", Category = category }
            );

            // Сохраняем изменения
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Service>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal("Service1", result.Data.Items[0].Name);
        }


        [Fact]
        public void ServiceReturnsCorrectPageNumber()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Service { Name = "Service1", Description = "Description1", Category = category },
                new Service { Name = "Service2", Description = "Description2", Category = category },
                new Service { Name = "Service3", Description = "Description3", Category = category },
                new Service { Name = "Service4", Description = "Description4", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 2).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Service>>>(result);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.Items.Count);
            Assert.Equal("Service4", result.Data.Items[0].Name);
        }

        [Fact]
        public void ServiceFiltersByCategory()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };
            var category2 = new Category { Name = "category2", NormalizedName = "category2" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Service { Name = "Service1", Description = "Description1", Category = category },
                new Service { Name = "Service2", Description = "Description2", Category = category },
                new Service { Name = "Service3", Description = "Description3", Category = category2 },
                new Service { Name = "Service4", Description = "Description4", Category = category2 }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync("category1").Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Service>>>(result);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.Equal("Service1", result.Data.Items[0].Name);
            Assert.Equal("Service2", result.Data.Items[1].Name);
        }

        [Fact]
        public void ServiceDoesNotAllowPageSizeGreaterThanMax()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };


            // Добавляем тестовые данные с обязательными полями
            context.Products.AddRange(
                new Service { Name = "Service1", Description = "Description1", Category = category },
                new Service { Name = "Service2", Description = "Description2", Category = category },
                new Service { Name = "Service3", Description = "Description3", Category = category },
                new Service { Name = "Service4", Description = "Description4", Category = category },
                new Service { Name = "Service5", Description = "Description1", Category = category },
                new Service { Name = "Service6", Description = "Description2", Category = category },
                new Service { Name = "Service7", Description = "Description3", Category = category },
                new Service { Name = "Service8", Description = "Description4", Category = category },
                new Service { Name = "Service9", Description = "Description1", Category = category },
                new Service { Name = "Service10", Description = "Description2", Category = category },
                new Service { Name = "Service11", Description = "Description3", Category = category },
                new Service { Name = "Service12", Description = "Description4", Category = category },
                new Service { Name = "Service13", Description = "Description1", Category = category },
                new Service { Name = "Service14", Description = "Description2", Category = category },
                new Service { Name = "Service15", Description = "Description3", Category = category },
                new Service { Name = "Service16", Description = "Description4", Category = category },
                new Service { Name = "Service17", Description = "Description1", Category = category },
                new Service { Name = "Service18", Description = "Description2", Category = category },
                new Service { Name = "Service19", Description = "Description3", Category = category },
                new Service { Name = "Service20", Description = "Description4", Category = category },
                new Service { Name = "Service21", Description = "Description1", Category = category },
                new Service { Name = "Service22", Description = "Description2", Category = category },
                new Service { Name = "Service23", Description = "Description3", Category = category },
                new Service { Name = "Service24", Description = "Description4", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 1, 50).Result; // Страница больше максимального размера

            // Assert
            Assert.IsType<ResponseData<ListModel<Service>>>(result);
            Assert.Equal(20, result.Data.Items.Count); // Три — это максимальное количество элементов на странице
        }

        [Fact]
        public void ServiceReturnsErrorWhenPageExceedsTotalPages()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            // Добавляем тестовые данные с обязательными полями
            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Service { Name = "Service1", Description = "Description1", Category = category },
                new Service { Name = "Service2", Description = "Description2", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 3).Result; // Страница больше общего количества страниц

            // Assert
            Assert.IsType<ResponseData<ListModel<Service>>>(result);
            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
