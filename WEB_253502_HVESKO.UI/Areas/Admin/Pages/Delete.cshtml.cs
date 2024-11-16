using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

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
            else
            {
                Service = service.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
