using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Wait_free
{
    public class BSW
    {
        public struct Register
        {
            public int value;
            public bool[] p;
            public bool toggle;
            public int[] snapshot;

        }

        private readonly int REGCOUNT;

        private bool[,] q;
        private Register[] r;
        private Stopwatch timer = new Stopwatch();
        private Dictionary<TimeSpan, int>[] logWrite;
        private Dictionary<TimeSpan, int[]> logRead = new Dictionary<TimeSpan, int[]>();

        public BSW(int regcount)
        {
            REGCOUNT = regcount;
            r = new Register[REGCOUNT];
            q = new bool[REGCOUNT, REGCOUNT];
            logWrite = new Dictionary<TimeSpan, int>[REGCOUNT];

            for (var i = 0; i < REGCOUNT; i++)
            {
                r[i].p = new bool[REGCOUNT];
                r[i].snapshot = new int[REGCOUNT];
                logWrite[i] = new Dictionary<TimeSpan, int>();
            }

            timer.Start();
        }

        public int[] Scan(int id, bool flag = true) 
        {
            var moved = new int[REGCOUNT]; 

            while (true)
            {

                for (var j = 0; j < REGCOUNT; j++)
                {
                    q[id, j] = r[j].p[id];
                }

                var a = r;
                var b = r;

                var condition = true;
                for (var j = 0; j < REGCOUNT; j++)
                {

                    if (a[j].p[id] != q[id, j] || b[j].p[id] != q[id, j] || a[j].toggle != b[j].toggle)
                    {
                        if (moved[j] == 1)
                        {
                            if (flag) logRead.Add(timer.Elapsed, b[j].snapshot);
                            return b[j].snapshot;
                        }

                        condition = false;
                        moved[j]++;
                    }

                }

                if (condition)
                {
                    var snapshot = new int[REGCOUNT];
                    for (var j = 0; j < REGCOUNT; j++)
                    {
                        snapshot[j] = b[j].value;
                    }

                    if (flag) logRead.Add(timer.Elapsed, snapshot);
                    return snapshot;
                }
            }
        }

        public void Update(int id, int value)
        {
            var f = new bool[REGCOUNT];
            for (var j = 0; j < REGCOUNT; j++)
            {
                f[j] = !q[j, id];
            }

            var snapshot = Scan(id, false);

            r[id].value = value;
            r[id].p = f;
            r[id].toggle = !r[id].toggle;
            r[id].snapshot = snapshot;

            logWrite[id].Add(timer.Elapsed, value);
        }

        public void Print()
        {
            for (var i = 0; i < REGCOUNT; i++)
            {
                Console.WriteLine("register #{0} and his log:", i);
                Console.WriteLine("({0}, [{1}], {2}, [{3}])", r[i].value, string.Join(",", r[i].p), r[i].toggle, string.Join(",", r[i].snapshot));
                foreach (var change in logWrite[i])
                {
                    Console.WriteLine(change);
                }
                Console.WriteLine("----------------------------");
            }
            Console.WriteLine("read-log:");
            foreach (var scan in logRead)
            {
                Console.Write("< values = (" + string.Join(", ", scan.Value));
                Console.WriteLine("), time = {0} >", scan.Key);
            }
            Console.WriteLine("----------------------------");
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bsw = new BSW(2);
            var random = new Random();
            var tasks = new Task[2];

            for (var i = 0; i < 22; i++)
            {
                var id = i % 2;
                var value = random.Next(1000);
                tasks[id] = Task.Run(() =>
                {
                    Console.WriteLine("write value = {0} in #{1} register", value, id);
                    bsw.Update(id, value);
                });

                if (i % 3 == 0)
                {
                    var count = i;
                    Task.Run(() =>
                    {
                        Console.WriteLine("read from {0} thread on {1} interation: ({2})", id, count, string.Join(", ", bsw.Scan(id)));
                    });
                }

                if (i % 2 == 1)
                {
                    Task.WaitAll(tasks);
                }
            }

            bsw.Print();

        }
    }
}