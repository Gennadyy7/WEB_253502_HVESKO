using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_HVESKO.Domain.Entities
{
    public class Cart
    {
        // Словарь для хранения элементов корзины, где ключ - ID продукта
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="service">Продукт, который нужно добавить</param>
        /// <param name="count">Количество добавляемого продукта</param>
        public virtual void AddToCart(Service service, int count)
        {
            if (CartItems.ContainsKey(service.ID))
            {
                // Если продукт уже есть в корзине, увеличиваем его количество
                CartItems[service.ID].Count += count;
            }
            else
            {
                // Если продукта нет, добавляем новый элемент корзины
                CartItems[service.ID] = new CartItem(service, count);
            }
        }

        /// <summary>
        /// Удалить объект из корзины по ID продукта
        /// </summary>
        /// <param name="serviceId">ID удаляемого продукта</param>
        public virtual void RemoveItem(int serviceId)
        {
            if (CartItems.ContainsKey(serviceId))
            {
                CartItems.Remove(serviceId);
            }
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count => CartItems.Sum(item => item.Value.Count);

        /// <summary>
        /// Общая стоимость всех объектов в корзине
        /// </summary>
        public decimal TotalPrice => CartItems.Sum(item => item.Value.Item.Price * item.Value.Count);
    }
}
