using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.API.Data;
using WEB_253502_HVESKO.API.Services.CategoryService;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly int _maxPageSize = 20; // Максимальный размер страницы

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            // Возвращаем данные
            return ResponseData<List<Category>>.Success(categories);
        }
    }
}
