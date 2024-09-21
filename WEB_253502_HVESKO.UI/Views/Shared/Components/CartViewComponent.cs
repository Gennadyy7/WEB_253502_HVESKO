using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Views.Shared.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartTotal = 0; // Стоимость корзины
            var itemCount = 0; // Количество товаров в корзине

            var model = new
            {
                Total = cartTotal,
                Count = itemCount
            };

            return View(model);
        }
    }
}