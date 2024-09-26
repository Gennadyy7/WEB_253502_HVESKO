using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.UI.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {ID=1, Name="Лазерная косметология", NormalizedName="laser-cosmetology"},
                new Category {ID=2, Name="Ультразвуковая кавитация", NormalizedName="ultrasound-cavitation"},
                new Category {ID=3, Name="Дермабразия", NormalizedName="dermabrasion"},
            };
            var result = ResponseData<List<Category>>.Success(categories);
            return Task.FromResult(result);
        }

    }
}
