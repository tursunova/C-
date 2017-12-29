using System;

namespace parallel_rbtree
{
    public class LockedRbNode
    {
        private bool _isRed;
        private LockedRbNode _left;
        private LockedRbNode _parent;
        private LockedRbNode _right;
        private int _value;


        public int Value { get => _value; set => this._value = value; }
        public LockedRbNode Left { get => _left; set => this._left = Left; }
        public LockedRbNode Right { get => _right; set => this._right = Right; }
        public LockedRbNode Parent { get => _parent; set => this._parent = Parent; }
        public bool IsRed { get => _isRed; set => this._isRed = IsRed; }

        public LockedRbNode()
        {
            _value = int.MinValue;
            _left = null;
            _right = null;
            _parent = null;
            _isRed = false;
        }

        public LockedRbNode(int value)
        {
            this._value = value;
            _left = new LockedRbNode();
            _right = new LockedRbNode();
            _parent = null;
            _isRed = true;
        }

        

        public void DisplayNode(LockedRbNode n)
        {
            Console.WriteLine(n.Value + " ");
        }
    }
}