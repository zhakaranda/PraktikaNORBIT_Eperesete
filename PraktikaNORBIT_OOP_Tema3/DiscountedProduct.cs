using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_OOP_Tema3
{
    class DiscountedProduct : Product
    {
        private decimal _discountSize;
        private decimal _promotionalPrice;

        public decimal DiscountSize
        {
            get { return _discountSize; }
            set { _discountSize = value; }
        }

        public decimal PromotionalPrice
        {
            get 
            { 
                return _promotionalPrice = _price * (1 - _discountSize/100);
            }
        }

        public DiscountedProduct(string name, string manufacturer, decimal price, int expirationDate, DateTime productionDate, 
            decimal discountSize) : base (name, manufacturer, price, expirationDate, productionDate) { DiscountSize = discountSize; }

        public override string ToString()
        {
            return base.ToString() +
                   $"\nСкидка: {DiscountSize}%\n" +
                   $"Цена товара со скидкой: {PromotionalPrice} рублей\n";
        }
    }
}
