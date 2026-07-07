using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_DataTypes_Zadanie1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите начальный вклад:");
                decimal initialDeposit = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Введите количество лет:");
                int years = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Введите годовую процентную ставку:");
                decimal interestRate = Convert.ToDecimal(Console.ReadLine());

                string result = CalculateProcent(initialDeposit, years, interestRate);
                Console.Write(result);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введите корректные числа.");
            }
        }

        /// <summary>
        /// Формирует строку с расчетом сложных процентов по годам.
        /// </summary>
        /// <param name="initialDeposit">Начальный вклад (положительное число).</param>
        /// <param name="years">Количество лет (положительное целое число).</param>
        /// <param name="interestRate">Годовая процентная ставка (положительное число).</param>
        /// <returns>Строка с расчетами по годам.</returns>
        static string CalculateProcent(decimal initialDeposit, int years, decimal interestRate)
        {
            if (initialDeposit <= 0 || years <= 0 || interestRate <= 0)
            {
                throw new ArgumentException("Все параметры должны быть положительными числами.");
            }

            StringBuilder result = new StringBuilder();
            decimal currentAmount = initialDeposit;
            decimal rateMultiplier = 1 + interestRate / 100;

            for (int year = 1; year <= years; year++)
            {
                currentAmount *= rateMultiplier;
                result.AppendLine($"Год {year}: {currentAmount:F2} руб.");
            }

            return result.ToString();
        }
    }
}

