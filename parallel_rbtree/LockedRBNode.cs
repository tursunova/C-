using System;

namespace parallel_rbtree
{
    public class LockedRbNode
    {
        private bool _isRed;
        private LockedRbNode _left;
        private LockedRbNode _parent;
        private LockedRbNode _right;
        private int? _value;

        public LockedRbNode()
        {
            _value = int.MinValue;
            _left = null;
            _right = null;
            _parent = null;
            _isRed = false;
        }

        public LockedRbNode(int? value)
        {
            this._value = value;
            _left = new LockedRbNode();
            _right = new LockedRbNode();
            _parent = null;
            _isRed = true;
        }

        public int? GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            this._value = value;
        }

        public LockedRbNode GetLeft()
        {
            return _left;
        }

        public void SetLeft(LockedRbNode left)
        {
            this._left = left;
        }

        public LockedRbNode GetRight()
        {
            return _right;
        }

        public void SetRight(LockedRbNode right)
        {
            this._right = right;
        }

        public LockedRbNode GetParent()
        {
            return _parent;
        }

        public void SetParent(LockedRbNode parent)
        {
            this._parent = parent;
        }

        public bool IsRed()
        {
            return _isRed;
        }

        public void SetRed(bool isRed)
        {
            this._isRed = isRed;
        }

        public void DisplayNode(LockedRbNode n)
        {
            Console.WriteLine(n.GetValue() + " ");
        }
    }
}