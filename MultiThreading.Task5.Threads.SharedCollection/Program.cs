/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    internal class Program
    {
        private static List<int> sharedCollection = new List<int>();
        private static object lockObject = new object();

        private static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Thread addThread = new Thread(AddElements);
            addThread.Start();

            Thread printThread = new Thread(PrintElements);
            printThread.Start();

            addThread.Join();
            printThread.Join();

            Console.WriteLine("All tasks completed.");
            Console.ReadLine();
        }

        private static void AddElements()
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread.Sleep(500);

                lock (lockObject)
                {
                    sharedCollection.Add(i);
                    Console.WriteLine($"Added {i} to the collection.");
                }
            }
        }

        private static void PrintElements()
        {
            for (int i = 1; i <= 10; i++)
            {
                lock (lockObject)
                {
                    if (sharedCollection.Count > 0)
                    {
                        Console.WriteLine("Current collection: " + string.Join(", ", sharedCollection));
                    }
                }
                Thread.Sleep(550);
            }
        }
    }
}