using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace PrimeNumbers
{
    class Program
    {
        private static int MaxNumOfThread = 3;
        static bool isPrime(int n)
        {
            if (n == 1) return false;
            for(int i = 2; i * i <= n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            int l = Convert.ToInt32(Console.ReadLine());
            int r = Convert.ToInt32(Console.ReadLine()) + 1;

            Console.WriteLine("Threads:");
            Stopwatch timer = Stopwatch.StartNew();
            List<int> ans = ThreadPrimes(l, r, 0);
            /*
            foreach(int i in ans)
            {
                Console.WriteLine(i);
            }
            */
            timer.Stop();
            Console.WriteLine("Time elapsed: {0}", timer.Elapsed);

            Console.WriteLine("Tasks:");
            ans = TaskPrimes(l, r);
            /*
            foreach(int i in ans)
            {
                Console.WriteLine(i);
            }
            */
            timer.Restart();
            Console.WriteLine("Time elapsed: {0}", timer.Elapsed);
            timer.Stop();


            Console.WriteLine("ThreadPool:");
            ans = ThreadPoolPrimes(l, r);
            /*
            foreach(int i in ans)
            {
                Console.WriteLine(i);
            }
            */
            timer.Restart();
            Console.WriteLine("Time elapsed: {0}", timer.Elapsed);
            timer.Stop();

            Console.ReadLine();
        }

        static List<int> TaskPrimes(int l, int r)
        {
            List<int> ans = new List<int>();

            if (r - l > 300)
            {
                int m = (l + r) / 2;

                List<int> ans1 = new List<int>();
                List<int> ans2 = new List<int>();

                Task<List<int>> leftTask = Task.Run(() => TaskPrimes(l, m));
                Task<List<int>> rightTask = Task.Run(() => TaskPrimes(m, r));

                Task.WaitAll(leftTask, rightTask);

                ans.AddRange(leftTask.Result);
                ans.AddRange(rightTask.Result);
            }
            else
            {
                for (int i = l; i < r; i++)
                {
                    if (isPrime(i)) ans.Add(i);
                }
            }
            return ans;
        }

        static List<int> ThreadPrimes(int l, int r, int numOfThread)
        {
            List<int> ans = new List<int>();

            if (numOfThread < MaxNumOfThread)
            {
                int m = (l + r) / 2;

                List<int> ans1 = new List<int>();
                List<int> ans2 = new List<int>();

                Thread firstThread = new Thread(() => ans1.AddRange(ThreadPrimes(l, m, numOfThread + 1)));
                Thread secondThread = new Thread(() => ans2.AddRange(ThreadPrimes(m , r, numOfThread + 2)));

                firstThread.Start();
                secondThread.Start();

                firstThread.Join();
                secondThread.Join();

                ans.AddRange(ans1);
                ans.AddRange(ans2);
            }
            else
            {
                for (int i = l; i < r; i++)
                {
                    if (isPrime(i)) ans.Add(i);
                }
            }
            return ans;
        }
        public static List<int> ThreadPoolPrimes(int l, int r)
        {
            List<int> res = new List<int>();

            var resEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(delegate
            {
                for (int i = l; i <= r; i++)
                {
                    if (isPrime(i)) res.Add(i);
                }
                resEvent.Set();
            });
            resEvent.WaitOne();

            return res;
        }
    }
}
