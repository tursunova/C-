using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace RedBlackTree
{

    class Program
    {
        static void Main(string[] args)
        {
            String line;
            String[] input = new String[100];
            int inputSize = 0;
            try
            {
                StreamReader file = new StreamReader("Sample.txt");
                
                line = file.ReadLine();
                input[0] = line;

                while (line != null)
                {
                    line = file.ReadLine();
                    inputSize++;
                    input[inputSize] = line;
                }
                
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            Request[] parseInput = ParseInput(inputSize,input);
            Run(parseInput);
            /*var tree = new RbTree<int, int>();
            tree.Insert(1, 1);
            tree.Insert(2, 2);
            tree.Insert(3, 3);
            tree.Insert(4, 4);
            tree.Insert(5, 5);
            var node = tree.Search(4);
            Console.WriteLine(node.key);
            //tree.Insert(6, 6);
            tree.Print();*/
            Console.ReadKey();
        }
        static void Run(Request[] request)
        {
            var tree = new RbTreeParallel<int, int>();
            var tasks = new List<Task>();
            var tasksInsert = new List<Task>();
            var tasksSearch = new List<Task>();
            var tasksDelete = new List<Task>();
           // Console.WriteLine(request.Length);
            for(int i = 0; i < request.Length; i++)
            {
                //Console.WriteLine(request[i].type+" "+request[i].key+" " +request[i].value);
                var key = request[i].key;
                var value = request[i].value;
                if (request[i].type == 1)
                {
                    tasks.Add(new Task(async () =>
                    {
                        await tree.Insert(key, value);
                    }));
                }
                else if (request[i].type == 3)
                {
                    tasks.Add(new Task(async () =>
                    {
                        var node = await tree.Search(key);
                        Console.WriteLine("search: " + node.key + " " + node.value);
                    }));
                }
            }
            foreach (var task in tasks)
            {
                if (task.Status == TaskStatus.Created)
                    /*return;
                else*/
                    task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();
            tree.Print();
        }
        static Request[] ParseInput(int inputSize,String[] input)
        {
            Request[] ans = new Request[inputSize];
            for(int i = 0; i < inputSize; i++)
            {
                int j = 0;
                String s = input[i];
                String st = "";
                while(s[j] != ' ')
                {
                    st += s[j];
                    j++;
                }
                String op = st.ToLower();
                j++;
                st = "";
                while(j < s.Length && s[j] != ' ')
                {
                    st += s[j];
                    j++;
                }
                int key = Convert.ToInt32(st);
                j++;
                st = "";
                while (j < s.Length)
                {
                    st += s[j];
                    j++;
                }
                int value = 0;
                if (st != "") value = Convert.ToInt32(st);
                int operation = 0;
                //Console.WriteLine(op + "+++");
                switch (op)
                {
                    case "insert":
                        operation = 1;
                        break;
                    case "search":
                        operation = 3;
                        break;
                    case "delete":
                        operation = 2;
                        break;
                    default:
                        break;
                }
                ans[i] = new Request(operation, key, value);
                //Console.WriteLine(operation + " " + key + " " + value);
            }
            return ans;
        }
    }
}
