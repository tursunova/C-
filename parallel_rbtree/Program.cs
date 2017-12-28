using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace parallel_rbtree
{
    class Program
    {
        
        public static void Main(String[] args) //throws InterruptedException
        {
            
            long start = 0, end = 0, duration = 0;
		    // ---- customize ----
		    int num_threads = 4;
		    int insert_nodes_per_thread = 10;
		    //Boolean visulize_locked_tree = false;
            Boolean visulize_lock_free_tree = true;
		    // ---------
		    //List<Thread> locked_threads = new List<Thread>();
		    List<Thread> lock_free_threads = new List<Thread>();
		    //lock Lock = new ReentrantLock();
		    //locked_threads.Clear();
		    lock_free_threads.Clear();
		    //LockedRBTree locked_tree = new LockedRBTree();
		    LockFreeRBTree lock_free_tree = new LockFreeRBTree();
            Random rand = new Random();
            Console.WriteLine(" -------- Insertion Test --------");
            Console.WriteLine("Each thread inserts: " + insert_nodes_per_thread + " nodes");
            Console.WriteLine("Lock-Based Threads:");
		    for (int i = 0; i < num_threads; i++) {
			    int[] values = new int[insert_nodes_per_thread];
			    for (int j = 0; j < insert_nodes_per_thread; j++) {
				   
				    values[j] = rand.Next(1000);
			    }
			   // locked_threads.Add(new insertThread(locked_tree, values, lock));
			    lock_free_threads.Add(new insertThread(lock_free_tree, values).getInstance());
		    }
		    //start = System.currentTimeMillis();
		    /*foreach(Thread thread in locked_threads){
			    thread.Start();
		    }
		    foreach(Thread thread in locked_threads){
			    thread.Join();
		    }*/
		
		    //end = System.currentTimeMillis();
		    //duration = end - start;
		    //Console.WriteLine("Lock-Based RBTree uses " +(double)duration + " ms");

            Console.WriteLine("Lock-Free Threads:");
		    //start = System.currentTimeMillis();
		    foreach(Thread thread in lock_free_threads){
			    thread.Start();
		    }
		    foreach(Thread thread in lock_free_threads){
			    thread.Join();
		    }
		    //end = System.currentTimeMillis();
		    //duration = end - start;
            Console.WriteLine("Lock-Free RBTree uses " +(double)duration + " ms");
		
		   /* if (visulize_locked_tree) {
			    locked_tree.breadth(locked_tree.root);
			    LockBasedRBTreeGUI locked_tree_gui = new LockBasedRBTreeGUI(locked_tree);
		    }*/
		
		    if (visulize_lock_free_tree) {
			    lock_free_tree.breadth(lock_free_tree.root);
			    //LockFreeRBTreeGUI lock_free_tree_gui = new LockFreeRBTreeGUI(lock_free_tree);	
		    }

            Console.WriteLine(" -------- Search Test --------");
            Console.WriteLine("Each thread search 10000 times");
            
		    /*locked_threads.clear();
		    for (int i = 0; i < num_threads; i++) {
			    locked_threads.add(new SearchThread(locked_tree,num_threads, insert_nodes_per_thread, lock));
		    }
		    start = System.currentTimeMillis(); 
		    for(Thread thread: locked_threads) {
			    thread.start();
		    }
		    for(Thread thread: locked_threads){
			    thread.join();
		    }
		    duration = System.currentTimeMillis() - start;
            
		    Console.WriteLine("Lock-Based RBTree uses: " +(double)duration + " ms");
		    */

		    lock_free_threads.Clear();
		    for (int i = 0; i<num_threads; i++) {
			    lock_free_threads.Add(new SearchThread(lock_free_tree, num_threads, insert_nodes_per_thread).getInstance());
		    }
		   // start = System.currentTimeMillis();
		    foreach(Thread thread in lock_free_threads) {
			    thread.Start();
		    }
		    foreach(Thread thread in lock_free_threads){
			    thread.Join();
		    }
            Console.ReadKey();
            // end = System.currentTimeMillis();
            //duration = end - start;
            // Console.WriteLine("Lock-Free RBTree uses " +(double)duration + " ms");
        }
    }
}
