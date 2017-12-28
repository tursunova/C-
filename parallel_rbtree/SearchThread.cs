using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace parallel_rbtree
{
    public class SearchThread  //extends Thread
    {

        public static int thread_id = 100;

        public int id;
        private RBTree rbTree;
        private int num;
        private int nodes;
        private Object Lock = null;
        private Thread thread;
	
	    public SearchThread(RBTree rbTree, int num, int nodes)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.num = num;
            this.nodes = nodes;
            this.Lock = null;//new Object();
        }

        public SearchThread(RBTree rbTree, int num, int nodes, Object Lock)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.num = num;
            this.nodes = nodes;
            this.Lock = Lock;
        }
        public Thread getInstance()
        {
            thread = new Thread(run);
            return thread;
        }
        public void run()
        {
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {

                int target = rand.Next(nodes * num);
                Console.WriteLine("Thread " + id + " search " + target);
                int? result = this.rbTree.search(target);



            }
        }
    }
}
