using System;
using System.Collections.Generic;
using System.Threading;

namespace parallel_rbtree
{
    internal class Program
    {
        public static void Main(string[] args) 
        {
            int numThreads = 4;
            int insertNodesPerThread = 10;
            bool visulizeLockFreeTree = true;
            List<Thread> lockFreeThreads = new List<Thread>();
            lockFreeThreads.Clear();
            LockFreeRbTree lockFreeTree = new LockFreeRbTree();
            Random rand = new Random();
            Console.WriteLine(" -------- Insertion Test --------");
            Console.WriteLine("Each thread inserts: " + insertNodesPerThread + " nodes");
            Console.WriteLine("Lock-Based Threads:");
            for (int i = 0; i < numThreads; i++)
            {
                int[] values = new int[insertNodesPerThread];
                for (int j = 0; j < insertNodesPerThread; j++)
                {
                    values[j] = rand.Next(1000);
                }
                lockFreeThreads.Add(new InsertThread(lockFreeTree, values).GetInstance());
            }
            Console.WriteLine("Lock-Free Threads:");
            //start = System.currentTimeMillis();
            foreach (Thread thread in lockFreeThreads)
            {
                thread.Start();
            }
            foreach (Thread thread in lockFreeThreads)
            {
                thread.Join();
            }
            

            if (visulizeLockFreeTree)
            {
                lockFreeTree.Breadth(lockFreeTree.Root);
            }

            Console.WriteLine(" -------- Search Test --------");
            Console.WriteLine("Each thread search 10000 times");
            

            lockFreeThreads.Clear();
            for (int i = 0; i < numThreads; i++)
            {
                lockFreeThreads.Add(
                    new SearchThread(lockFreeTree, numThreads, insertNodesPerThread).GetInstance());
            }
            foreach (Thread thread in lockFreeThreads)
            {
                thread.Start();
            }
            foreach (Thread thread in lockFreeThreads)
            {
                thread.Join();
            }
        }
    }
}