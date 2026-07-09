using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_OOP_Tema3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите название товара:");
            string name = Console.ReadLine();

            Console.WriteLine("Введите производителя:");
            string manufacturer = Console.ReadLine();

            Console.WriteLine("Введите цену на товар:");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Введите срок годности (в днях):");
            int expirationDate = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите дату производства (дд.мм.гггг):");
            DateTime productionDate = Convert.ToDateTime(Console.ReadLine());

            Console.WriteLine("Введите размер скидки:");
            decimal discountSize = Convert.ToDecimal(Console.ReadLine());

            DiscountedProduct discountedProduct = new DiscountedProduct(name, manufacturer, price, expirationDate, productionDate, discountSize);

            Console.WriteLine("\nИнформация о товаре со скидкой:");
            Console.WriteLine(discountedProduct);
        }
    }
}
