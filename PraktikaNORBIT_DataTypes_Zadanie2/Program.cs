using System;
using System.Text;

namespace PraktikaNORBIT_DataTypes_Zadanie2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите длину диагонали(положительное нечётное целое число не меньше 3):");
                int n = Convert.ToInt32(Console.ReadLine());

                string result = BuildDiamond(n);
                Console.Write(result);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введите корректное число.");
            }
        }

        /// <summary>
        /// Формирует ромб из символов X с пустым центром.
        /// </summary>
        /// <param name="n">Длина каждой диагонали (положительное нечётное целое число не меньше 3).</param>
        /// <returns>Ромб из символов X.</returns>
        /// <exception cref="ArgumentException">Если n <= 0, n чётное или n < 3.</exception>
        static string BuildDiamond(int n)
        {
            if (n <= 0 || n % 2 == 0 || n < 3)
            {
                throw new ArgumentException("Параметр должен быть положительными нечетным целым числом не меньше 3.");
            }

            StringBuilder result = new StringBuilder();
            int center = n / 2;

            for (int row = 0; row < n; row++)
            {
                int distanceFromCenter = Math.Abs(center - row);

                for (int col = 0; col < n; col++)
                {
                    bool isOnLeftDiagonal = (col == distanceFromCenter);
                    bool isOnRightDiagonal = (col == n - 1 - distanceFromCenter);
                    bool isCenterCell = (row == center) && (col == center);

                    if ((isOnLeftDiagonal || isOnRightDiagonal) && !isCenterCell)
                    {
                        result.Append("X");
                    }
                    else
                    {
                        result.Append(" ");
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    }  
}


