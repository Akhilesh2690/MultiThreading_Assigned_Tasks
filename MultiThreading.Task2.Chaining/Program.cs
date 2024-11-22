/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var taskChain = Task.Run(() => GenerateRandomArray())
                                .ContinueWith(task => MultiplyArray(task.Result), TaskContinuationOptions.OnlyOnRanToCompletion)
                                .ContinueWith(task => SortArray(task.Result), TaskContinuationOptions.OnlyOnRanToCompletion)
                                .ContinueWith(task => CalculateAverage(task.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

            await taskChain;

            Console.ReadLine();
        }

        private static int[] GenerateRandomArray()
        {
            Random rand = new Random();
            int[] arr = new int[10];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rand.Next(1, 100);
            }
            Console.WriteLine("Generated Random Array:");
            Console.WriteLine(string.Join(", ", arr));
            return arr;
        }

        private static int[] MultiplyArray(int[] array)
        {
            Random rand = new Random();
            int multiplier = rand.Next(1, 10);
            int[] result = array.Select(x => x * multiplier).ToArray();

            Console.WriteLine($"\n Array after multiplying by {multiplier}:");
            Console.WriteLine(string.Join(", ", result));
            return result;
        }

        private static int[] SortArray(int[] array)
        {
            Array.Sort(array);
            Console.WriteLine("\n Array after sorting:");
            Console.WriteLine(string.Join(", ", array));
            return array;
        }

        private static void CalculateAverage(int[] array)
        {
            double average = array.Average();
            Console.WriteLine($"\n Average value of the array: {average}");
        }
    }
}