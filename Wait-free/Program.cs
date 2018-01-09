using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Wait_free
{
    public class Bsw
    {
        private readonly int _regcount;
        private readonly Dictionary<TimeSpan, int[]> _logRead = new Dictionary<TimeSpan, int[]>();
        private readonly Dictionary<TimeSpan, int>[] _logWrite;

        private readonly bool[,] _q;
        private readonly Register[] _r;
        private readonly Stopwatch _timer = new Stopwatch();

        public Bsw(int regcount)
        {
            _regcount = regcount;
            _r = new Register[_regcount];
            _q = new bool[_regcount, _regcount];
            _logWrite = new Dictionary<TimeSpan, int>[_regcount];

            for (int i = 0; i < _regcount; i++)
            {
                _r[i].P = new bool[_regcount];
                _r[i].Snapshot = new int[_regcount];
                _logWrite[i] = new Dictionary<TimeSpan, int>();
            }

            _timer.Start();
        }

        public int[] Scan(int id, bool flag = true)
        {
            int[] moved = new int[_regcount];

            while (true)
            {
                for (int j = 0; j < _regcount; j++)
                {
                    _q[id, j] = _r[j].P[id];
                }

                Register[] a = (Register[])_r.Clone();
                Register[] b = (Register[])_r.Clone();

                bool condition = true;
                for (int j = 0; j < _regcount; j++)
                {
                    if (a[j].P[id] != _q[id, j] || b[j].P[id] != _q[id, j] || a[j].Toggle != b[j].Toggle)
                    {
                        if (moved[j] == 1)
                        {
                            if (flag)
                            {
                                _logRead.Add(_timer.Elapsed, b[j].Snapshot);
                            }
                            return b[j].Snapshot;
                        }

                        condition = false;
                        moved[j]++;
                    }
                }

                if (condition)
                {
                    int[] snapshot = new int[_regcount];
                    for (int j = 0; j < _regcount; j++)
                    {
                        snapshot[j] = b[j].Value;
                    }

                    if (flag)
                    {
                        _logRead.Add(_timer.Elapsed, snapshot);
                    }
                    return snapshot;
                }
            }
        }

        public void Update(int id, int value)
        {
            bool[] f = new bool[_regcount];
            for (int j = 0; j < _regcount; j++)
            {
                f[j] = !_q[j, id];
            }

            int[] snapshot = Scan(id, false);

            _r[id].Value = value;
            _r[id].P = f;
            _r[id].Toggle = !_r[id].Toggle;
            _r[id].Snapshot = snapshot;

            _logWrite[id].Add(_timer.Elapsed, value);
        }

        public void Print()
        {
            for (int i = 0; i < _regcount; i++)
            {
                Console.WriteLine("register #{0} and his log:", i);
                Console.WriteLine("({0}, [{1}], {2}, [{3}])", _r[i].Value, string.Join(",", _r[i].P), _r[i].Toggle,
                    string.Join(",", _r[i].Snapshot));
                foreach (KeyValuePair<TimeSpan, int> change in _logWrite[i])
                {
                    Console.WriteLine(change);
                }
                Console.WriteLine("----------------------------");
            }
            Console.WriteLine("read-log:");
            foreach (KeyValuePair<TimeSpan, int[]> scan in _logRead)
            {
                Console.Write("< values = (" + string.Join(", ", scan.Value));
                Console.WriteLine("), time = {0} >", scan.Key);
            }
            Console.WriteLine("----------------------------");
        }

        public struct Register
        {
            public int Value;
            public bool[] P;
            public bool Toggle;
            public int[] Snapshot;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Bsw bsw = new Bsw(2);
            Random random = new Random();
            Task[] tasks = new Task[2];
            List<Task> taskReader = new List<Task>();

            for (int i = 0; i < 22; i++)
            {
                int id = i % 2;
                int value = random.Next(1000);
                tasks[id] = Task.Run(() =>
                {
                    Console.WriteLine("write value = {0} in #{1} register", value, id);
                    bsw.Update(id, value);
                });

                if (i % 3 == 0)
                {
                    int count = i;
                    taskReader.Add(Task.Run(() =>
                    {
                        Console.WriteLine("read from {0} thread on {1} interation: ({2})", id, count,
                            string.Join(", ", bsw.Scan(id)));
                    }));
                }

                if (i % 2 == 1)
                {
                    Task.WaitAll(tasks);
                }
            }
            Task.WaitAll(taskReader.ToArray());
            bsw.Print();

        }
    }
}