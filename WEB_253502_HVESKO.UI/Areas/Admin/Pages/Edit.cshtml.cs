using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI.Data;
using WEB_253502_HVESKO.UI.Services.CategoryService;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _productService.GetProductByIdAsync(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            Service = service.Data;
            var categories = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categories.Data, "ID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var service = await _productService.GetProductByIdAsync(Service.ID);
            Service.ImagePath = service.Data.ImagePath;
            await _productService.UpdateProductAsync(Service.ID, Service, Image);

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            var service = await _productService.GetProductByIdAsync(id);
            return service == null;
        }
    }
}
