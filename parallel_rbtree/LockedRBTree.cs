using System;


namespace parallel_rbtree
{
    public class LockedRbTree : IRbTree
    {
        public LockedRbNode Root;

        public int Size = 0;

        public LockedRbTree()
        {
            Root = null;
        }

        public int? Search(int value) 
        {
            if (Root == null)
            {
                return int.MinValue;
            }
            LockedRbNode temp = Root;
            while (temp != null && temp.GetValue() > 0)
            {
                if (value < temp.GetValue())
                {
                    temp = temp.GetLeft();
                }
                else if (value > temp.GetValue())
                {
                    temp = temp.GetRight();
                }
                else
                {
                    return temp.GetValue();
                }
            }
            if (temp == null)
            {
                return null;
            }
            return temp.GetValue();
            
        }

        public void Insert(int value) 
        {
            LockedRbNode insertedNode = PhysicallyInsert(value);
            RbInsertFixup(insertedNode);
        }

        private LockedRbNode PhysicallyInsert(int value)
        {
            LockedRbNode insertedNode = new LockedRbNode(value);
            if (Root == null || Root.GetValue() < 0)
            {
                Root = insertedNode;
                Root.SetRed(false);
                insertedNode.SetLeft(new LockedRbNode());
                insertedNode.SetRight(new LockedRbNode());
                insertedNode.GetLeft().SetParent(insertedNode);
                insertedNode.GetRight().SetParent(insertedNode);
                return insertedNode;
            }

            LockedRbNode temp = Root;
            while (temp.GetValue() >= 0)
            {
                if (value < temp.GetValue())
                {
                    temp = temp.GetLeft();
                }
                else if (value > temp.GetValue())
                {
                    temp = temp.GetRight();
                }
                else
                {
                    return null;
                }
            }

            if (temp == temp.GetParent().GetLeft())
            {
                temp.GetParent().SetLeft(insertedNode);
            }
            else
            {
                temp.GetParent().SetRight(insertedNode);
            }
            insertedNode.SetParent(temp.GetParent());
            insertedNode.GetLeft().SetParent(insertedNode);
            insertedNode.GetRight().SetParent(insertedNode);
            return insertedNode;
        }

        private void RbInsertFixup(LockedRbNode x)
        {
            while (x != Root && x.GetParent().IsRed())
            {
                if (x.GetParent() == x.GetParent().GetParent().GetLeft())
                {
                    LockedRbNode uncle = x.GetParent().GetParent().GetRight();
                    if (uncle.IsRed())
                    {
                        x.GetParent().SetRed(false);
                        uncle.SetRed(false);
                        x.GetParent().GetParent().SetRed(true);
                        x = x.GetParent().GetParent();
                    }
                    else
                    {
                        if (x == x.GetParent().GetRight())
                        {
                            x = x.GetParent();
                            LeftRotate(x);
                        }
                        x.GetParent().SetRed(false);
                        x.GetParent().GetParent().SetRed(true);
                        RightRotate(x.GetParent().GetParent());
                    }
                }
                else
                {
                    LockedRbNode uncle = x.GetParent().GetParent().GetLeft();
                    if (uncle.IsRed())
                    {
                        x.GetParent().SetRed(false);
                        uncle.SetRed(false);
                        x.GetParent().GetParent().SetRed(true);
                        x = x.GetParent().GetParent();
                    }
                    else
                    {
                        if (x == x.GetParent().GetLeft())
                        {
                            x = x.GetParent();
                            RightRotate(x);
                        }
                        x.GetParent().SetRed(false);
                        x.GetParent().GetParent().SetRed(true);
                        LeftRotate(x.GetParent().GetParent());
                    }
                }
            }
            Root.SetRed(false);
        }

        private void LeftRotate(LockedRbNode x)
        {
            if (x == null)
            {
                return;
            }
            LockedRbNode y = x.GetRight();
            x.SetRight(y.GetLeft());
            if (y.GetLeft() != null)
            {
                y.GetLeft().SetParent(x);
            }
            y.SetParent(x.GetParent());
            if (x.GetParent() == null)
            {
                Root = y;
            }
            else
            {
                if (x == x.GetParent().GetLeft())
                {
                    x.GetParent().SetLeft(y);
                }
                else
                {
                    x.GetParent().SetRight(y);
                }
            }
            y.SetLeft(x);
            x.SetParent(y);
        }

        private void RightRotate(LockedRbNode y)
        {
            if (y == null)
            {
                return;
            }
            LockedRbNode x = y.GetLeft();
            y.SetLeft(x.GetRight());
            if (x.GetRight() != null)
            {
                x.GetRight().SetParent(y);
            }
            x.SetParent(y.GetParent());
            if (y.GetParent() == null)
            {
                Root = x;
            }
            else
            {
                if (y == y.GetParent().GetLeft())
                {
                    y.GetParent().SetLeft(x);
                }
                else
                {
                    y.GetParent().SetRight(x);
                }
            }
            x.SetRight(y);
            y.SetParent(x);
        }


        public int Getheight(LockedRbNode root)
        {
            if (root == null)
            {
                return 0;
            }
            return Math.Max(Getheight(root.GetLeft()), Getheight(root.GetRight())) + 1;
        }


        public void PreOrder(LockedRbNode n)
        {
            if (n == null)
            {
                return;
            }
            //n.displayNode(n);
            PreOrder(n.GetLeft());
            PreOrder(n.GetRight());
        }

        public void Breadth(LockedRbNode n)
        {
            if (n == null)
            {
                return;
            }
            //n.displayNode(n);
            PreOrder(n.GetLeft());
            PreOrder(n.GetRight());
        }
    }
}