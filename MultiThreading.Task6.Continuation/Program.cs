/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Task taskA = Task.Run(() =>
            {
                Console.WriteLine("Parent Task A running...");
                Thread.Sleep(1000);
                Console.WriteLine("Parent Task A completed.");
            });

            taskA.ContinueWith(t =>
            {
                Console.WriteLine("Continuation Task A executed Regardless of the result).");
            }, TaskContinuationOptions.None);

            Task taskB = Task.Run(() =>
            {
                Console.WriteLine("Task B Running on Thread ID " + Thread.CurrentThread.ManagedThreadId);
                throw new InvalidOperationException("An error occurred in the task");
            });

            taskB.ContinueWith((t) =>
            {
                Console.WriteLine("Continuation Task B executed on Faulted:  (OnFaulted) - Exception: " + t.Exception?.InnerException?.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task taskC = Task.Run(() =>
            {
                Console.WriteLine("Task C Canceled Running on Thread ID " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(100);
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                Console.WriteLine("Parent Task canceled.");
            }, cancellationTokenSource.Token);

            taskC.ContinueWith((t) =>
            {
                Console.WriteLine("Continuation Task C executed on Cancled: Continuation (OnCanceled) - Task was canceled.");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task.WhenAll(taskA, taskB, taskC).Wait();
            Console.ReadLine();
        }
    }
}