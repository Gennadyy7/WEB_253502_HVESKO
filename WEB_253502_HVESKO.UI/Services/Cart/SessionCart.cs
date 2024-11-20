using WEB_253502_HVESKO.UI.Extensions;

namespace WEB_253502_HVESKO.UI.Services.Cart
{
    using WEB_253502_HVESKO.Domain.Entities;
    public class SessionCart : Cart
    {
        private readonly ISession _session;

        public SessionCart(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
            LoadCartFromSession();
        }

        // Загружаем данные корзины из сессии
        private void LoadCartFromSession()
        {
            var cart = _session.Get<Cart>("cart");
            if (cart != null)
            {
                CartItems = cart.CartItems;
            }
        }

        // Сохраняем данные корзины в сессию
        private void SaveCartToSession()
        {
            _session.Set("cart", this);
        }

        public override void AddToCart(Service service, int count = 1)
        {
            base.AddToCart(service, count);
            SaveCartToSession();
        }

        public override void RemoveItem(int serviceId)
        {
            base.RemoveItem(serviceId);
            SaveCartToSession();
        }

        public override void ClearAll()
        {
            base.ClearAll();
            SaveCartToSession();
        }
    }
}
