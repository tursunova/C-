using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_rbtree
{
    public class LockFreeRBNode
    {
        private int? value;
        private LockFreeRBNode left;
        private LockFreeRBNode right;
        private LockFreeRBNode parent;
        private Boolean isRed;
        public AtomicBoolean flag; 

        public LockFreeRBNode()
        {
            this.value = Int32.MinValue;
            this.left = null;
            this.right = null;
            this.parent = null;
            this.isRed = false;
            this.flag = new AtomicBoolean(false);
        }

        public LockFreeRBNode(int? value)
        {
            this.value = value;
            this.left = new LockFreeRBNode();
            this.right = new LockFreeRBNode();
            this.parent = null;
            this.isRed = true;
            this.flag = new AtomicBoolean(false);
        }

        public int? getValue()
        {
            return value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }

        public LockFreeRBNode getLeft()
        {
            return left;
        }

        public void setLeft(LockFreeRBNode left)
        {
            this.left = left;
        }

        public LockFreeRBNode getRight()
        {
            return right;
        }

        public void setRight(LockFreeRBNode right)
        {
            this.right = right;
        }

        public LockFreeRBNode getParent()
        {
            return parent;
        }

        public void setParent(LockFreeRBNode parent)
        {
            this.parent = parent;
        }

        public Boolean IsRed()
        {
            return isRed;
        }

        public void setRed(Boolean isRed)
        {
            this.isRed = isRed;
        }

        public int height()
        {
            if (this == null) return 0;
            return (1 + Math.Max(this.getLeft().height(), this.getRight().height()));
        }
        public void displayNode()
        {
            Console.WriteLine(value + " ");
        }
    }
}
