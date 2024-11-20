using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_HVESKO.Domain.Entities;
using WEB_253502_HVESKO.UI.Services.ProductService;

namespace WEB_253502_HVESKO.UI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;

        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }

        [Route("add/{id:int}")]
        public async Task<IActionResult> Add(int id, string returnUrl, int count = 1)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data, count);
            }

            return Redirect(returnUrl);
        }

        [Route("remove")]
        public IActionResult Remove(int id, string returnUrl)
        {
            _cart.RemoveItem(id);
            return Redirect(returnUrl);
        }

        [Route("clear")]
        public IActionResult Clear(string returnUrl)
        {
            _cart.ClearAll();
            return Redirect(returnUrl);
        }

        public IActionResult Index()
        {
            return View(_cart);
        }
    }
}
