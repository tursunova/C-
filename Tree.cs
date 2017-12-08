using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Tree
{
    public class Node<K, V> where K : IComparable
    {
        public Node<K, V> parent = null;
        public Node<K, V> leftChild = null;
        public Node<K, V> rightChild = null;
        public K key;
        public V value;

        public Node(K newKey, V newValue)
        {
            this.key = newKey;
            this.value = newValue;
        }

        public bool IsRoot()
        {
            return parent == null;
        }

        public bool IsLeaf()
        {
            return leftChild == null && rightChild == null;
        }

        public override bool Equals(object obj)
        {
            Node<K, V> other = obj as Node<K, V>;
            if (other == null)
            {
                return false;
            }
            return key.Equals(other.key) && value.Equals(other.value);
        }

    }
    public class BST<K, V> where K : IComparable
    {
        private Node<K, V> root;
        public async Task<Node<K,V>> Search(K key)
        {
            return await Task.Run(() =>
            {
                Node<K,V> currNode = root;
                while (currNode != null && !currNode.key.Equals(key))
                {
                    if (currNode.key.CompareTo(key) < 0)
                    {
                        currNode = currNode.leftChild;
                    }
                    else 
                    {
                        currNode = currNode.rightChild;
                    }
                }
                return currNode;
            });
        }

        public async Task Insert(K key, V value)
        {
            await Task.Run(() =>
            {
                if (root == null)
                {
                    root = new Node<K, V>(key, value); 
                    return;
                }
                Node<K, V> currNode = root;
                while (currNode != null)
                {
                    if (currNode.key.CompareTo(key) < 0)
                    {
                        if (currNode.leftChild != null)
                        {
                            currNode = currNode.leftChild;
                        }
                        else
                        {
                            Node<K, V> newNode  = new Node<K,V> (key, value);
                            newNode.parent = currNode;
                            currNode.leftChild = newNode;
                            return;
                        }
                    }
                    else if (currNode.key.CompareTo(key) > 0)
                    {
                        if (currNode.rightChild != null)
                        {
                            currNode = currNode.rightChild;
                        }
                        else
                        {
                            Node<K, V> newNode = new Node<K, V>(key, value);
                            newNode.parent = currNode;
                            currNode.rightChild = newNode;
                            return;
                        }
                    }
                    else if (currNode.key.Equals(key))
                    {
                        currNode.value = value;
                    }
                }

            });
        }
        public async Task Delete(K key)
        {
            await Task.Run(async () =>
            {
                Node<K, V> deleteNode = await Search(key);
                Node<K, V> parent = deleteNode.parent;
                if (deleteNode.IsLeaf()) 
                {
                    if (parent.leftChild == deleteNode)
                    {
                        parent.leftChild = null;
                    } else if (parent.rightChild == deleteNode)
                    {
                        parent.rightChild = null;
                    }
                } else if (deleteNode.leftChild == null || deleteNode.rightChild == null) 
                {
                    if (deleteNode.leftChild == null)
                    {
                        if (parent.leftChild == deleteNode)
                        {
                            parent.leftChild = deleteNode.rightChild;
                        }
                        else if (parent.rightChild == deleteNode)
                        {
                            parent.rightChild = deleteNode.rightChild;
                        }
                    } else if (deleteNode.rightChild == null)
                    {
                        if (parent.leftChild == deleteNode)
                        {
                            parent.leftChild = deleteNode.leftChild;
                        }
                        else if (parent.rightChild == deleteNode)
                        {
                            parent.rightChild = deleteNode.leftChild;
                        }
                    }
                } else 
                {
                    var successor = Next(deleteNode);
                    deleteNode.key = successor.key;
                    if (successor.parent.leftChild == successor)
                    {
                        successor.parent.leftChild = successor.rightChild;
                        if (successor.rightChild!= null)
                        {
                            successor.rightChild.parent = successor.parent;
                        }
                    } else
                    {
                        successor.parent.rightChild = successor.rightChild;
                        if (successor.rightChild != null)
                        {
                            successor.rightChild.parent = successor.parent;
                        }
                    }
                } 
            });
        }
        private Node<K,V> Next(Node<K,V> node)
        {
            Node<K, V> current = root;
            Node<K, V> successor = null;
            while (current != null)
            {
                if (current.key.CompareTo(node) > 0)
                {
                    successor = current;
                    current = current.leftChild;
                } else
                {
                    current = current.leftChild;
                }
            }
            return successor;
        }
        public void Print()
        {
            Queue<Node<K, V>> q = new Queue<Node<K, V>>();
            if (root != null) q.Enqueue(root);
            while (q.Any())
            {
                Node<K, V> v = q.Dequeue();
                Console.WriteLine(v.key + " " + v.value);
                if (v.leftChild != null) q.Enqueue(v.leftChild);
                if (v.rightChild != null) q.Enqueue(v.rightChild);
            }   

        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            var tree = new BST<int, int>();
            //var tasks = new List<Task>();
            var tasksInsert = new List<Task>();
            var tasksSearch = new List<Task>();
            var tasksDelete = new List<Task>();
            tasksInsert.Add(new Task(async () =>
            {
                await tree.Insert(1, 1);
            }));
            tasksInsert.Add(new Task(async () =>
            {
                await tree.Insert(2, 1);
            }));
            tasksInsert.Add(new Task(async () =>
            {
                await tree.Insert(1, 5);
            }));

            foreach (var task in tasksInsert)
            {
                task.Start();
            }
            Task.WaitAll(tasksInsert.ToArray());
            //Thread.Sleep(3000);
            tasksInsert.Clear();
            tasksSearch.Add(new Task(async () =>
            {
                Console.WriteLine("nofqafqaf");
                var node = await tree.Search(2);
                Console.WriteLine("nofqafqaf");
                //Console.ReadKey();
                if (node == null)
                    Console.WriteLine("not found");
                else
                    Console.WriteLine(node.key + " " + node.value);
            }));
            foreach (var task in tasksSearch)
            {
                task.Start();
            }
            Task.WaitAll(tasksSearch.ToArray());
            //Thread.Sleep(3000);
            tasksSearch.Clear();
            Thread.Sleep(3000);

            tree.Print();

            Console.ReadKey();
        }
        
    }
}
