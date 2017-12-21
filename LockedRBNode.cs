using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_rbtree
{
    public class LockedRBNode
    {
        private int? value;
        private LockedRBNode left;
        private LockedRBNode right;
        private LockedRBNode parent;
        private Boolean isRed;

        public LockedRBNode()
        {
            this.value = Int32.MinValue;
            this.left = null;
            this.right = null;
            this.parent = null;
            this.isRed = false;
        }

        public LockedRBNode(int? value)
        {
            this.value = value;
            this.left = new LockedRBNode();
            this.right = new LockedRBNode();
            this.parent = null;
            this.isRed = true;
        }

        public int? getValue()
        {
            return value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }

        public LockedRBNode getLeft()
        {
            return left;
        }

        public void setLeft(LockedRBNode left)
        {
            this.left = left;
        }

        public LockedRBNode getRight()
        {
            return right;
        }

        public void setRight(LockedRBNode right)
        {
            this.right = right;
        }

        public LockedRBNode getParent()
        {
            return parent;
        }

        public void setParent(LockedRBNode parent)
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
        public void displayNode(LockedRBNode n)
        {
            Console.WriteLine(n.getValue() + " ");
        }
    }
}
