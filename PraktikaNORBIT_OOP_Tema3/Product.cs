using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_OOP_Tema3
{
    abstract class Product
    {
        protected string _name;
        protected string _manufacturer;
        protected decimal _price;
        protected int _expirationDate;
        protected DateTime _productionDate;

        public string Name
        {
            get { return _name; }
        }

        public string Manufacturer
        {  
            get { return _manufacturer; } 
        }

        public decimal Price
        {
            get { return  _price; }
            set { _price = value; }
        }

        public int ExpirationDate
        {
            get { return _expirationDate; }
        }

        public DateTime ProductionDate
        {
            get { return _productionDate; }
        }

        public Product(string name, string manufacturer, decimal price, int expirationDate, DateTime productionDate)
        {
            _name = name;
            _manufacturer = manufacturer;
            _price = price;
            _expirationDate = expirationDate;
            _productionDate = productionDate;
        }

        public override string ToString()
        {
            return $"Товар: {Name}\n" +
                   $"Производитель: {Manufacturer}\n" +
                   $"Цена: {Price} рублей\n" +
                   $"Срок годности: {ExpirationDate} дней\n" +
                   $"Дата производства: {ProductionDate:dd.MM.yyyy}\n" +
                   $"Годен до: {ProductionDate.AddDays(ExpirationDate):dd.MM.yyyy}";
        }
    }
}
