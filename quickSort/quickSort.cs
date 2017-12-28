using System;
using System.Threading;

namespace quickSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int i;
            int[] a = new int[n];
            for (i = 0; i < n; i++)
            {
                a[i] = Convert.ToInt32(Console.ReadLine());
            }
            qTuple t = new qTuple
            {
                array = a,
                start = 0,
                finish = n-1
            };
            qSort(t);
            //Console.WriteLine("WWWWWWWWWWw");
            for (i = 0; i < n; i++)
            {
               Console.WriteLine(a[i]);
            }

            Console.ReadKey();
        }
        static void qSort(Object o)
        {
            qTuple t = (qTuple)o;
            int start = t.start, finish = t.finish;
            int[] a = t.array;
            int l = start, r = finish, m = (l + r) / 2;
            if (l >= r) return;
            while (l <= r)
            {
                while (a[l] < a[m])
                {
                    l++;
                }
                while (a[m] < a[r])
                {
                    r--;
                }
                if (l <= r)
                {
                    int c = a[l];
                    a[l] = a[r];
                    a[r] = c;
                    l++;
                    r--;
                }
            }
            qTuple t_l = new qTuple
            {
                array = a,
                start = start,
                finish = r
            };
            qTuple t_r = new qTuple
            {
                array = a,
                start = l,
                finish = finish
            };
            Thread th = new Thread(new ParameterizedThreadStart(qSort));
            th.Start(t_l);
            qSort(t_r);
        }
        private class qTuple
        {
            public int[] array; 
            public int start;
            public int finish;
        }
    }
}
