using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace parallel_rbtree
{

    public class insertThread //extends Thread
    {

        public static int thread_id = 0;
        public int id;
        private RBTree rbTree;
        private int[] values;
        //private Lock lock;
	
	    public insertThread(RBTree rbTree, int[] values)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.values = values;
            //this.lock = null;
        }
        /*
        public insertThread(RBTree rbTree, int[] values, Lock lock)
        {
            this.id = thread_id++;
            this.rbTree = rbTree;
            this.values = values;
            this.lock = lock;
        }
        */
        public void run()
        {
            foreach (int value in values)
            {
                //System.out.println("Thread "+id+" add "+ value);
               /* if (lock != null)
                    {
                        lock.lock () ;
                    }
                    */
                try
                {
                    this.rbTree.insert(value);
                }
                catch(Exception e) //catch (NullPointerException e)
                {
                    // 
                }
                /*if (lock != null)
                    {
                        lock.unlock();
                    }
                    */
            }
        }
    }

}
