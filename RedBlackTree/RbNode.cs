using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public enum Color
    {
        red, black
    }
    public class RbNode<K, V> where K : IComparable
    {
        public RbNode<K, V> parent = null;
        public RbNode<K, V> leftChild = null;
        public RbNode<K, V> rightChild = null;
        public Color color = Color.black;
        public K key;
        public V value;

        public RbNode(K newKey, V newValue, Color newColor)
        {
            this.key = newKey;
            this.value = newValue;
            this.color = newColor;
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
            RbNode<K, V> other = obj as RbNode<K, V>;
            if (other == null)
            {
                return false;
            }
            return key.Equals(other.key) && value.Equals(other.value) && color.Equals(other.color);
        }

    }
}
