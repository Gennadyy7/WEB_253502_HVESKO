using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_HVESKO.Domain.Entities
{
    public class CartItem
    {
        public Service Item { get; set; } // Объект продукта
        public int Count { get; set; }    // Количество продуктов в корзине

        public CartItem(Service item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
