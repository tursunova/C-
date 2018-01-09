using System;
using System.Threading;

namespace parallel_rbtree
{
    public class InsertThread
    {
        public static int ThreadId;
        public int Id;
        private object _lock;
        private readonly IRbTree _rbTree;
        private Thread _thread;
        private readonly int[] _values;

        public InsertThread(IRbTree rbTree, int[] values)
        {
            Id = ThreadId++;
            this._rbTree = rbTree;
            this._values = values;
            _lock = new object();
        }

        public InsertThread(IRbTree rbTree, int[] values, object Lock)
        {
            Id = ThreadId++;
            this._rbTree = rbTree;
            this._values = values;
            this._lock = Lock;
        }

        public Thread GetInstance()
        {
            _thread = new Thread(Run);
            return _thread;
        }


        public void Run()
        {
            foreach (int value in _values)
            {
                Console.WriteLine("Thread " + Id + " add " + value);

                _rbTree.Insert(value);
                
            }
        }
    }
}