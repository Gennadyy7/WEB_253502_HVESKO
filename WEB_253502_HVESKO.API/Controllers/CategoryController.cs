using Microsoft.AspNetCore.Mvc;
using WEB_253502_HVESKO.API.Services.CategoryService;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.Domain.Models;

namespace WEB_253502_HVESKO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var response = await _categoryService.GetCategoryListAsync();
            if (!response.Successfull)
                return NotFound(response.ErrorMessage);

            return Ok(response);
        }
    }
}
