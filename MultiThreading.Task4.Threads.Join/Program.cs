/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 *
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class Program
    {
        private static Semaphore semaphore;

        private static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine("Running with Thread and Join:");
            CreateThreadRecursively(10);

            Console.WriteLine("Running with ThreadPool and Semaphore:");
            CreateThreadPoolTask(10);

            Console.ReadLine();
        }

        private static void CreateThreadRecursively(int state)
        {
            if (state <= 0) return;

            Thread thread = new Thread(() =>
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started with state: {state}");
                state--;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} state after decrement: {state}");
                CreateThreadRecursively(state);
            });

            thread.Start();
            thread.Join();
        }

        private static void CreateThreadPoolTask(int number)
        {
            semaphore = new Semaphore(0, 10);

            for (int i = 0; i < 10; i++)
            {
                int state = number - i;
                ThreadPool.QueueUserWorkItem(PerformTask, state);
            }

            for (int i = 0; i < 10; i++)
            {
                semaphore.WaitOne();
            }
        }

        private static void PerformTask(object state)
        {
            int number = (int)state;
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started with state: {number}");
            number--;
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} state after decrement: {number}");
            semaphore.Release();
        }
    }
}