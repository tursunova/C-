using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public class RbTree<K, V> where K : IComparable
    {
        private RbNode<K, V> root;

        public RbNode<K,V> Search(K key)
        {
            RbNode<K,V> currNode = root;
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
        }
        public void Insert(K key, V value)
        {
            if (this.root == null)
            {
                this.root = new RbNode<K, V>(key, value, Color.black);
                return;
            }
            var currNode = root;
            RbNode<K,V> parent = null;
            while (currNode != null)
            {
                parent = currNode;
                if (currNode.key.CompareTo(key) < 0)
                {
                    if (currNode.leftChild != null)
                    {
                        currNode = currNode.leftChild;
                    }
                    else
                    {
                        var newNode = new RbNode<K, V>(key, value, Color.red);
                        newNode.parent = currNode;
                        currNode.leftChild = newNode;
                        FixInsert(newNode);
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
                        var newNode = new RbNode<K, V>(key, value, Color.red);
                        newNode.parent = currNode;
                        currNode.rightChild = newNode;
                        FixInsert(newNode);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            return; 
        }
        private void FixInsert(RbNode<K, V> node)
        {
            while (node.parent != null && node.parent.color == Color.red)
            {
                RbNode<K, V> tmp;
                if (node.parent.Equals(node.parent?.parent?.leftChild))
                {
                    tmp = node.parent?.parent?.rightChild;
                    if (tmp != null && tmp.color == Color.red)
                    {
                        node.parent.color = Color.black;
                        tmp.color = Color.black;
                        node.parent.parent.color = Color.red;
                        node = node.parent.parent;
                    }
                    else
                    {
                        if (node.Equals(node.parent?.rightChild))
                        {
                            node = node.parent;
                            LeftRotation(node);
                        }
                        node.parent.color = Color.black;
                        node.parent.parent.color = Color.red;
                        RightRotation(node.parent.parent);
                    }
                }
                else
                {
                    tmp = node.parent?.parent?.leftChild;
                    if (tmp != null && tmp.color == Color.red)
                    {
                        node.parent.color = Color.black;
                        tmp.color = Color.black;
                        node.parent.parent.color = Color.red;
                        node = node.parent.parent;
                    }
                    else
                    {
                        if (node.Equals(node.parent?.leftChild))
                        {
                            node = node.parent;
                            RightRotation(node);
                        }
                        node.parent.color = Color.black;
                        node.parent.parent.color = Color.red;
                        LeftRotation(node.parent.parent);
                    }
                }
            }
            root.color = Color.black;
        }
        private void LeftRotation(RbNode<K,V> node) {
            var newNode = node.rightChild;
            node.rightChild = newNode.leftChild;
            if (newNode.leftChild != null)
            {
                newNode.leftChild.parent = node;
            }
            newNode.parent = node.parent;

            if (node.parent == null)
            {
                this.root = newNode;
            }
            else
            {
                if (node.Equals(node.parent.leftChild))
                {
                    node.parent.leftChild = newNode;
                }
                else
                {
                    node.parent.rightChild = newNode;
                }
            }
            newNode.leftChild = node;
            node.parent = newNode;
        }
        private void RightRotation(RbNode<K,V> node)
        {
            var newNode = node.leftChild;
            node.leftChild = newNode.rightChild;
            if (newNode.rightChild != null)
            {
                newNode.rightChild.parent = node;
            }
            newNode.parent = node.parent;

            if (node.parent == null)
            {
                this.root = newNode;
            }
            else
            {
                if (node.Equals(node.parent.leftChild))
                {
                    node.parent.leftChild = newNode;
                }
                else
                {
                    node.parent.rightChild = newNode;
                }
            }
            newNode.rightChild = node;
            node.parent = newNode;
        }
        public void Print()
        {
            Queue<RbNode<K, V>> q = new Queue<RbNode<K, V>>();
            if (root != null) q.Enqueue(root);
            while (q.Any())
            {
                RbNode<K, V> v = q.Dequeue();
                Console.WriteLine(v.key + " " + v.value + " " + v.color);
                if (v.leftChild != null) q.Enqueue(v.leftChild);
                if (v.rightChild != null) q.Enqueue(v.rightChild);
            }
        }
    }
}
