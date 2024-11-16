using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI.Services.CategoryService;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet()
        {
            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.CreateProductAsync(Service, null);

            return RedirectToPage("./Index");
        }
    }
}
