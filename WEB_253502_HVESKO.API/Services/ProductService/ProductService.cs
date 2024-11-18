using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Service>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Products.AsQueryable();

            if (categoryNormalizedName == "Все")
                categoryNormalizedName = null;

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                query = query.Where(p => p.Category.NormalizedName == categoryNormalizedName);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNo > totalPages)
                return ResponseData<ListModel<Service>>.Error("No such page");

            var products = await query
                .OrderBy(p => p.ID)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dataList = new ListModel<Service>
            {
                Items = products,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return ResponseData<ListModel<Service>>.Success(dataList);
        }

        public async Task<ResponseData<Service>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return ResponseData<Service>.Error("Product not found");
            }

            return ResponseData<Service>.Success(product);
        }

        public async Task UpdateProductAsync(int id, Service product, IFormFile? formFile)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct != null)
            {
                // Обновление данных продукта
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;

                // Если передан файл изображения, обновите изображение
                if (product.ImagePath != null)
                {
                    existingProduct.ImagePath = product.ImagePath;
                }
                existingProduct.CategoryId = product.CategoryId;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Service>> CreateProductAsync(Service product, IFormFile? formFile)
        {
            if (formFile != null)
            {
                var imageUrl = await SaveImageAsync(product.ID, formFile);
                product.ImagePath = imageUrl;
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return ResponseData<Service>.Success(product);
        }

        private async Task<string> SaveImageAsync(int id, IFormFile formFile)
        {
            // Логика сохранения изображения в папку wwwroot/Images
            var filePath = Path.Combine("wwwroot/Images", $"{id}_{formFile.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return $"/Images/{id}_{formFile.FileName}";
        }
    }
}
