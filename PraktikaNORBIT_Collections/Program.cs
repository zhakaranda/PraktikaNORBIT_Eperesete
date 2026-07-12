using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraktikaNORBIT_Collections
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stack = new SmartStack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Console.WriteLine($"Вершина: {stack.Peek()}"); 
            Console.WriteLine($"Количество: {stack.Count}"); 

            stack.PushRange(new[] { 4, 5, 6 });
            

            Console.WriteLine($"Вершина: {stack.Peek()}"); 

            Console.WriteLine("Обход стека:");
            foreach (var item in stack)
            {
                Console.Write(item + " "); 
            }

            Console.WriteLine($"\nЭлемент по глубине 2: {stack[2]}"); 
        }
    }
}
