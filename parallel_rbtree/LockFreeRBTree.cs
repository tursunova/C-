using System;
using System.Collections.Generic;

namespace parallel_rbtree
{
    public class LockFreeRbTree : IRbTree
    {
        public LockFreeRbNode Root;
        public int Size = 0;

        public LockFreeRbTree()
        {
            Root = new LockFreeRbNode();
        }

        public int? Search(int value)
        {
            if (Root == null)
            {
                return null;
            }
            LockFreeRbNode temp = Root;
            while (temp != null && temp.Value >= 0)
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
            /*if (value == null)
            {
                throw new ArgumentException("Parameter cannot be null", "original");
            }*/
            LockFreeRbNode insertedNode = new LockFreeRbNode(value);
            LockFreeRbNode temp1, temp2;
            insertedNode.Flag.Set(true);
            while (true)
            {
                temp1 = Root;
                temp2 = null;
                while (temp1.Value >= 0)
                {
                    temp2 = temp1;
                    if (value < temp1.Value)
                    {
                        temp1 = temp1.Left;
                    }
                    else
                    {
                        temp1 = temp1.Right;
                    }
                }
                if (!SetupLocalAreaForInsert(temp2))
                {
                    temp2.Flag.Set(false);
                }
                else
                {
                    break;
                }
            }

            insertedNode.Parent = temp2;
            if (temp2 == null)
            {
                Root = insertedNode;
            }
            else if (value < temp2.Value)
            {
                temp2.Left = (insertedNode);
            }
            else
            {
                temp2.Right = (insertedNode);
            }
            insertedNode.Left.Parent = (insertedNode);
            insertedNode.Right.Parent = (insertedNode);
            insertedNode.IsRed = true;
            RbInsertFixup(insertedNode);
        }

        private bool SetupLocalAreaForInsert(LockFreeRbNode x)
        {
            if (x == null)
            {
                return true;
            }
            LockFreeRbNode parent = x.Parent;
            LockFreeRbNode uncle;
            if (parent == null)
            {
                return true;
            }
            if (!x.Flag.CompareAndSet(false, true))
            {
                return false;
            }
            if (!parent.Flag.CompareAndSet(false, true))
            {
                return false;
            }
            if (parent != x.Parent)
            {
                parent.Flag.Set(false);
                return false;
            }   
            if (x == x.Parent.Left)
            {
                uncle = x.Parent.Right;
            }
            else
            {
                uncle = x.Parent.Left;
            }
            if (uncle != null && !uncle.Flag.CompareAndSet(false, true))
            {
                x.Parent.Flag.Set(false);
                return false;
            }
            return true;
        }

        private void RbInsertFixup(LockFreeRbNode x)
        {
            LockFreeRbNode temp, parent, uncle = null, gradparent = null;
            parent = x.Parent;
            List<LockFreeRbNode> localArea = new List<LockFreeRbNode>
            {
                x,
                parent
            };

            if (parent != null)
            {
                gradparent = parent.Parent;
            }

            if (gradparent != null)
            {
                if (gradparent.Left == parent)
                {
                    uncle = gradparent.Right;
                }
                else
                {
                    uncle = gradparent.Left;
                }
            }

            localArea.Add(uncle);
            localArea.Add(gradparent);

            while (x.Parent != null && x.Parent.IsRed)
            {
                parent = x.Parent;
                gradparent = gradparent.Parent;

                if (x.Parent == x.Parent.Parent.Left)
                {
                    temp = x.Parent.Parent.Right;
                    uncle = temp;
                    localArea.Add(x);
                    localArea.Add(parent);
                    localArea.Add(gradparent);
                    localArea.Add(uncle);

                    if (temp.IsRed)
                    {
                        x.Parent.IsRed = false;
                        temp.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = MoveLocalAreaUpward(x, localArea);
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
                    temp = x.Parent.Parent.Left;
                    uncle = temp;

                    localArea.Add(x);
                    localArea.Add(parent);
                    localArea.Add(gradparent);
                    localArea.Add(uncle);

                    if (temp.IsRed)
                    {
                        x.Parent.IsRed = false;
                        temp.IsRed = false;
                        x.Parent.Parent.IsRed = true;
                        x = MoveLocalAreaUpward(x, localArea);
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

            foreach (LockFreeRbNode node in localArea)
            {
                if (node != null)
                {
                    node.Flag.Set(false);
                }
            }
        }

        private LockFreeRbNode MoveLocalAreaUpward(LockFreeRbNode x, List<LockFreeRbNode> working)
        {
            LockFreeRbNode parent = x.Parent;
            LockFreeRbNode grandparent = parent.Parent;
            LockFreeRbNode uncle;
            if (parent == grandparent.Left)
            {
                uncle = grandparent.Right;
            }
            else
            {
                uncle = grandparent.Left;
            }

            LockFreeRbNode updatedX, updatedParent = null, updatedUncle = null, updatedGrandparent = null;
            updatedX = grandparent;

            while (true && updatedX.Parent != null)
            {
                updatedParent = updatedX.Parent;
                if (!updatedParent.Flag.CompareAndSet(false, true))
                {
                    continue;
                }
                updatedGrandparent = updatedParent.Parent;
                if (updatedGrandparent == null)
                {
                    break;
                }
                if (!updatedGrandparent.Flag.CompareAndSet(false, true))
                {
                    updatedParent.Flag.Set(false);
                    continue;
                }
                if (updatedParent == updatedGrandparent.Left)
                {
                    updatedUncle = updatedGrandparent.Right;
                }
                else
                {
                    updatedUncle = updatedGrandparent.Left;
                }

                if (updatedUncle != null && !updatedUncle.Flag.CompareAndSet(false, true))
                {
                    updatedGrandparent.Flag.Set(false);
                    updatedParent.Flag.Set(false);
                    continue;
                }
                break;
            }

            working.Add(updatedX);
            working.Add(updatedParent);
            working.Add(updatedGrandparent);
            working.Add(updatedUncle);

            return updatedX;
        }

        private void LeftRotate(LockFreeRbNode x)
        {
            if (x == null)
            {
                return;
            }
            LockFreeRbNode y = x.Right;
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

        private void RightRotate(LockFreeRbNode y)
        {
            if (y == null)
            {
                return;
            }
            LockFreeRbNode x = y.Left;
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


        public int GetHeight(LockFreeRbNode root)
        {
            if (root == null)
            {
                return 0;
            }
            return Math.Max(GetHeight(root.Left), GetHeight(root.Right)) + 1;
        }


        public void PreOrder(LockFreeRbNode n)
        {
            if (n == null)
            {
                return;
            }
            n.DisplayNode();
            PreOrder(n.Left);
            PreOrder(n.Right);
        }

        public void Breadth(LockFreeRbNode n)
        {
            if (n == null)
            {
                return;
            }
            n.DisplayNode();
            PreOrder(n.Left);
            PreOrder(n.Right);
        }
    }
}