/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */

using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    internal class Program
    {
        private const int TaskAmount = 100;
        private const int MaxIterationsCount = 1000;

        private static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            HundredTasks();

            Console.ReadLine();
        }

        private static void HundredTasks()
        {
            Task[] tasks = new Task[TaskAmount];

            for (int i = 0; i < TaskAmount; i++)
            {
                int taskId = i;

                tasks[i] = Task.Run(() =>
                {
                    for (int j = 1; j <= MaxIterationsCount; j++)
                    {
                        Output(taskId, j);
                    }
                });
            }

            Task.WhenAll(tasks).Wait();
            Console.WriteLine("All tasks are completed.");
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}