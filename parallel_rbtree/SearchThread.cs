using System;
using System.Threading;

namespace parallel_rbtree
{
    public class SearchThread 
    {
        public static int ThreadId = 100;

        public int Id;
        private object _lock;
        private readonly int _nodes;
        private readonly int _num;
        private readonly IRbTree _rbTree;
        private Thread _thread;

        public SearchThread(IRbTree rbTree, int num, int nodes)
        {
            Id = ThreadId++;
            this._rbTree = rbTree;
            this._num = num;
            this._nodes = nodes;
            _lock = null; //new Object();
        }

        public SearchThread(IRbTree rbTree, int num, int nodes, object Lock)
        {
            Id = ThreadId++;
            this._rbTree = rbTree;
            this._num = num;
            this._nodes = nodes;
            this._lock = Lock;
        }

        public Thread GetInstance()
        {
            _thread = new Thread(Run);
            return _thread;
        }

        public void Run()
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int target = rand.Next(_nodes * _num);
                Console.WriteLine("Thread " + Id + " search " + target);
                int? result = _rbTree.Search(target);
                if (result != null) Console.WriteLine("FIND!!! " + target);
            }
        }
    }
}