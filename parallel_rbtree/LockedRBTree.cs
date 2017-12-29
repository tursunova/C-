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
            while (temp != null && temp.Value > 0)
            {
                if (value < temp.Value)
                {
                    temp = temp.Left;
                }
                else if (value > temp.Value)
                {
                    temp = temp.Right;
                }
                else
                {
                    return temp.Value;
                }
            }
            if (temp == null)
            {
                return null;
            }
            return temp.Value;
            
        }

        public void Insert(int value) 
        {
            LockedRbNode insertedNode = PhysicallyInsert(value);
            RbInsertFixup(insertedNode);
        }

        private LockedRbNode PhysicallyInsert(int value)
        {
            LockedRbNode insertedNode = new LockedRbNode(value);
            if (Root == null || Root.Value < 0)
            {
                Root = insertedNode;
                Root.IsRed = false;
                insertedNode.Left = new LockedRbNode();
                insertedNode.Right = new LockedRbNode();
                insertedNode.Left.Parent = insertedNode;
                insertedNode.Right.Parent = insertedNode;
                return insertedNode;
            }

            LockedRbNode temp = Root;
            while (temp.Value >= 0)
            {
                if (value < temp.Value)
                {
                    temp = temp.Left;
                }
                else if (value > temp.Value)
                {
                    temp = temp.Right;
                }
                else
                {
                    return null;
                }
            }

            if (temp == temp.Parent.Left)
            {
                temp.Parent.Left = insertedNode;
            }
            else
            {
                temp.Parent.Right = insertedNode;
            }
            insertedNode.Parent = temp.Parent;
            insertedNode.Left.Parent = insertedNode;
            insertedNode.Right.Parent = insertedNode;
            return insertedNode;
        }

        private void RbInsertFixup(LockedRbNode x)
        {
            while (x != Root && x.Parent.IsRed)
            {
                if (x.Parent == x.Parent.Parent.Left)
                {
                    LockedRbNode uncle = x.Parent.Parent.Right;
                    if (uncle.IsRed)
                    {
                        x.Parent.IsRed = false;
                        uncle.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Right)
                        {
                            x = x.Parent;
                            LeftRotate(x);
                        }
                        x.Parent.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        RightRotate(x.Parent.Parent);
                    }
                }
                else
                {
                    LockedRbNode uncle = x.Parent.Parent.Left;
                    if (uncle.IsRed)
                    {
                        x.Parent.IsRed = false;
                        uncle.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Left)
                        {
                            x = x.Parent;
                            RightRotate(x);
                        }
                        x.Parent.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        LeftRotate(x.Parent.Parent);
                    }
                }
            }
            Root.IsRed = false;
        }

        private void LeftRotate(LockedRbNode x)
        {
            if (x == null)
            {
                return;
            }
            LockedRbNode y = x.Right;
            x.Right = y.Left;
            if (y.Left != null)
            {
                y.Left.Parent = x;
            }
            y.Parent = x.Parent;
            if (x.Parent == null)
            {
                Root = y;
            }
            else
            {
                if (x == x.Parent.Left)
                {
                    x.Parent.Left = y;
                }
                else
                {
                    x.Parent.Right = y;
                }
            }
            y.Left = x;
            x.Parent = y;
        }

        private void RightRotate(LockedRbNode y)
        {
            if (y == null)
            {
                return;
            }
            LockedRbNode x = y.Left;
            y.Left = x.Right;
            if (x.Right != null)
            {
                x.Right.Parent = y;
            }
            x.Parent = y.Parent;
            if (y.Parent == null)
            {
                Root = x;
            }
            else
            {
                if (y == y.Parent.Left)
                {
                    y.Parent.Left = x;
                }
                else
                {
                    y.Parent.Right = x;
                }
            }
            x.Right = y;
            y.Parent = x;
        }


        public int Getheight(LockedRbNode root)
        {
            if (root == null)
            {
                return 0;
            }
            return Math.Max(Getheight(root.Left), Getheight(root.Right)) + 1;
        }


        public void PreOrder(LockedRbNode n)
        {
            if (n == null)
            {
                return;
            }
            //n.displayNode(n);
            PreOrder(n.Left);
            PreOrder(n.Right);
        }

        public void Breadth(LockedRbNode n)
        {
            if (n == null)
            {
                return;
            }
            //n.displayNode(n);
            PreOrder(n.Left);
            PreOrder(n.Right);
        }
    }
}