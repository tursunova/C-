using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Delete(K key)
        {
            var currNode = root;
            while (currNode != null && !currNode.key.Equals(key))
            {
                if (currNode.key.CompareTo(key) < 0)
                    currNode = currNode.leftChild;
                else
                    currNode = currNode.rightChild;
            }
            if (currNode == null)
            {
                return;
            }
            var deletedNode = currNode;
            deletedNode.parent = currNode.parent;
            if ((deletedNode.leftChild != null) && (deletedNode.rightChild != null)) {
                deletedNode = currNode.rightChild;
                while (deletedNode.leftChild != null) {
                    deletedNode = deletedNode.leftChild;
                }
            }
            RbNode<K, V> node;
            if (deletedNode.leftChild != null)
                node = deletedNode.leftChild;
            else
                node = deletedNode.rightChild;
            
            //node.parent = deletedNode.parent;//?????????????????
            if (deletedNode.parent != null)
            {
                if (deletedNode.parent.leftChild.Equals(deletedNode)) {
                    deletedNode.parent.leftChild = node;
                } else {
                    deletedNode.parent.rightChild = node;
                }
            }
            else
            {
                root = node;
            }
            if (!deletedNode.Equals(currNode))
            {
                currNode.key = deletedNode.key;
                currNode.value = deletedNode.value;
            }
            if (deletedNode.color == Color.black)
            {
                if (node != null)
                {
                    FixDelete(node);
                }
                else if (root != null)
                {
                    FixDelete(deletedNode, true);
              }
            }
            return;
        }

        private void FixDelete(RbNode<K,V> node, Boolean isNull = false)
        {
            var x = isNull;
            var currNode = node;
            while (x || !currNode.Equals(root) && currNode.color == Color.black)
            {
                if (currNode.parent.leftChild.Equals(currNode) || currNode.parent.leftChild == null) {
                    var brotherNode = currNode.parent.rightChild;
                    if (brotherNode.color == Color.red) {

                        brotherNode.color = Color.black;
                        currNode.parent.color = Color.red;
                        LeftRotation(currNode.parent);
                        brotherNode = currNode.parent.rightChild;
                    }
                    if (!(brotherNode.leftChild != null &&
                            brotherNode.rightChild != null &&
                            brotherNode.rightChild.color == Color.red &&
                            brotherNode.leftChild.color == Color.red)) {
                        brotherNode.color = Color.red;
                        currNode = currNode.parent;
                    }
                    else
                    {
                        if (brotherNode.leftChild != null && brotherNode.leftChild.color == Color.red) {
                            brotherNode.leftChild.color = Color.black;
                            brotherNode.color = Color.red;
                            RightRotation(brotherNode);
                        }
                        brotherNode.color = currNode.parent.color;
                        currNode.parent.color = Color.black;
                        brotherNode.rightChild.color = Color.black;
                        LeftRotation(currNode.parent);
                        currNode = root;
                    }
                }
                else
                {
                    var brotherNode = currNode.parent.leftChild;
                    if (brotherNode.color == Color.red) {
                        brotherNode.color = Color.black;
                        currNode.parent.color = Color.red;
                        RightRotation(currNode.parent);
                        brotherNode = currNode.parent.leftChild;
                    }
                    if (!(brotherNode.leftChild != null &&
                            brotherNode.rightChild != null &&
                            brotherNode.rightChild.color == Color.red &&
                            brotherNode.leftChild.color == Color.red)) {
                        brotherNode.color = Color.red;
                        currNode = currNode.parent;
                    } else {
                        if (brotherNode.rightChild != null && brotherNode.rightChild.color == Color.red) {
                            brotherNode.rightChild.color = Color.black;
                            brotherNode.color = Color.red;
                            LeftRotation(brotherNode);
                        }
                        brotherNode.color = currNode.parent.color;
                        currNode.parent.color = Color.black;
                        brotherNode.leftChild.color = Color.black;
                        RightRotation(currNode.parent);
                        currNode = root;
                    }
                }
                x = false;
            }
            currNode.color = Color.black;
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
