using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices; // MethodImpl

namespace parallel_rbtree
{

    public class LockedRBTree : RBTree
    {

        public int size = 0;
        public LockedRBNode root;

        public LockedRBTree()
        {
            this.root = null;
        }

        public int? search(int? value) //synchronized
        {
            if (root == null) return Int32.MinValue;
            LockedRBNode temp = root;
            while (temp != null && temp.getValue() > 0)
            {
                if (value < temp.getValue())
                {
                    temp = temp.getLeft();
                }
                else if (value > temp.getValue())
                {
                    temp = temp.getRight();
                }
                else
                {
                    return temp.getValue();
                }
            }
            if (temp == null)
                return null;
            else
                return temp.getValue();

            //return temp == null ? null : temp.getValue();
        }

        public void insert(int? value) // synchronized
        {
            LockedRBNode insertedNode = physicallyInsert(value);
            rbInsertFixup(insertedNode);
        }

        private LockedRBNode physicallyInsert(int? value)
        {
            LockedRBNode insertedNode = new LockedRBNode(value);
            if (root == null || root.getValue() < 0)
            {
                root = insertedNode;
                root.setRed(false);
                insertedNode.setLeft(new LockedRBNode());
                insertedNode.setRight(new LockedRBNode());
                insertedNode.getLeft().setParent(insertedNode);
                insertedNode.getRight().setParent(insertedNode);
                return insertedNode;
            }

            LockedRBNode temp = root;
            while (temp.getValue() >= 0)
            {
                if (value < temp.getValue())
                {
                    temp = temp.getLeft();
                }
                else if (value > temp.getValue())
                {
                    temp = temp.getRight();
                }
                else
                {
                    return null;
                }
            }

            if (temp == temp.getParent().getLeft())
            {
                temp.getParent().setLeft(insertedNode);
            }
            else
            {
                temp.getParent().setRight(insertedNode);
            }
            insertedNode.setParent(temp.getParent());
            insertedNode.getLeft().setParent(insertedNode);
            insertedNode.getRight().setParent(insertedNode);
            return insertedNode;
        }

        private void rbInsertFixup(LockedRBNode x)
        {
            while (x != this.root && x.getParent().IsRed())
            {
                if (x.getParent() == x.getParent().getParent().getLeft())
                {
                    LockedRBNode uncle = x.getParent().getParent().getRight();
                    if (uncle.IsRed())
                    {
                        // Case 1
                        x.getParent().setRed(false);
                        uncle.setRed(false);
                        x.getParent().getParent().setRed(true);
                        x = x.getParent().getParent();
                    }
                    else
                    {
                        if (x == x.getParent().getRight())
                        {
                            // Case 2
                            x = x.getParent();
                            leftRotate(x);
                        }
                        // Case 3
                        x.getParent().setRed(false);
                        x.getParent().getParent().setRed(true);
                        rightRotate(x.getParent().getParent());
                    }
                }
                else
                {
                    LockedRBNode uncle = x.getParent().getParent().getLeft();
                    if (uncle.IsRed())
                    {
                        //Case 1
                        x.getParent().setRed(false);
                        uncle.setRed(false);
                        x.getParent().getParent().setRed(true);
                        x = x.getParent().getParent();
                    }
                    else
                    {
                        if (x == x.getParent().getLeft())
                        {
                            // Case 2
                            x = x.getParent();
                            rightRotate(x);
                        }
                        // Case 3
                        x.getParent().setRed(false);
                        x.getParent().getParent().setRed(true);
                        leftRotate(x.getParent().getParent());
                    }
                }
            }
            this.root.setRed(false);
        }

        private void leftRotate(LockedRBNode x)
        {
            if (x == null) return;
            LockedRBNode y = x.getRight();
            x.setRight(y.getLeft());
            if (y.getLeft() != null)
            {
                y.getLeft().setParent(x);
            }
            y.setParent(x.getParent());
            if (x.getParent() == null) this.root = y;
            else
            {
                if (x == x.getParent().getLeft())
                    x.getParent().setLeft(y);
                else
                    x.getParent().setRight(y);
            }
            y.setLeft(x);
            x.setParent(y);
        }

        private void rightRotate(LockedRBNode y)
        {
            if (y == null) return;
            LockedRBNode x = y.getLeft();
            y.setLeft(x.getRight());
            if (x.getRight() != null)
            {
                x.getRight().setParent(y);
            }
            x.setParent(y.getParent());
            if (y.getParent() == null) this.root = x;
            else
            {
                if (y == y.getParent().getLeft())
                    y.getParent().setLeft(x);
                else
                    y.getParent().setRight(x);
            }
            x.setRight(y);
            y.setParent(x);
        }



        public int getheight(LockedRBNode root)
        {
            if (root == null)
                return 0;
            return Math.Max(getheight(root.getLeft()), getheight(root.getRight())) + 1;
        }


        public void preOrder(LockedRBNode n)
        {

            if (n == null)
                return;
            //n.displayNode(n);
            preOrder(n.getLeft());
            preOrder(n.getRight());
        }

        public void breadth(LockedRBNode n)
        {

            if (n == null)
                return;
            //n.displayNode(n);
            preOrder(n.getLeft());
            preOrder(n.getRight());
        }

    } 
}
