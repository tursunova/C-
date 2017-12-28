using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace parallel_rbtree
{

    public class insertThread
    {

        public static int thread_id = 0;
        public int id;
        private RBTree rbTree;
        private int[] values;
        private Thread thread; 
        private Object Lock;
	
	    public insertThread(RBTree rbTree, int[] values)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.values = values;
            this.Lock = new Object();
        }
        
        public insertThread(RBTree rbTree, int[] values, object Lock)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.values = values;
            this.Lock = Lock;

        }

        public Thread getInstance()
        {
            thread = new Thread(run);
            return thread;
        }


        public void run()
        {
            foreach (int value in values)
            {
                Console.WriteLine("Thread "+id+" add "+ value);
               
                    try
                    {
                        this.rbTree.insert(value);
                    }
                    catch (Exception e) //catch (NullPointerException e)
                    {

                    }
                
                  
            }
        }
    }

}
