using Microsoft.AspNetCore.Mvc;
using WEB_253502_HVESKO.UI.Extensions;
using WEB_253502_HVESKO.UI.Services.CategoryService;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Controllers
{
    [Route("Catalog")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet("{category?}")]
        public async Task<IActionResult> Index([FromServices] IConfiguration config, string category, int pageNo = 1)
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);
            ViewBag.Categories = categoriesResponse.Data;
            ViewBag.CurrentCategory = categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            // Проверяем, является ли запрос AJAX-запросом
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CatalogPartial", productResponse.Data); // Возвращаем частичное представление
            }

            return View(productResponse.Data);
        }
    }
}
