﻿using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace SemaphoreSample
{
    class Program
    {
        static void Main(string[] args)
        {
            int taskCount = 6;
            int semaphoreCount = 3;
            var semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
            var tasks = new Task[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() => TaskMain(semaphore));
            }

            Task.WaitAll(tasks);

            WriteLine("All tasks finished");
        }

        private static void TaskMain(SemaphoreSlim semaphore)
        {
            bool isCompleted = false;
            while (!isCompleted)
            {
                if (semaphore.Wait(600))
                {
                    try
                    {
                        WriteLine($"Task {Task.CurrentId} locks the semaphore");
                        Task.Delay(2000).Wait();
                    }
                    finally
                    {
                        WriteLine($"Task {Task.CurrentId} releases the semaphore");
                        semaphore.Release();
                        isCompleted = true;
                    }
                }
                else
                {
                    WriteLine($"Timeout for task {Task.CurrentId}; wait again");
                }
            }
        }
    }
}
