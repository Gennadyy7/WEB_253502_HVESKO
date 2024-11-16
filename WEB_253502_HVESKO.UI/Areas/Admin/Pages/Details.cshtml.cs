using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

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
    }
}
