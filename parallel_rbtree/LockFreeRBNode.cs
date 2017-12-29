using System;

namespace parallel_rbtree
{
    public class LockFreeRbNode
    {
        public AtomicBoolean Flag;
        private bool _isRed;
        private LockFreeRbNode _left;
        private LockFreeRbNode _parent;
        private LockFreeRbNode _right;
        private int _value;

        public int Value { get => _value; set => this._value = value; }
        public LockFreeRbNode Left { get => _left; set => this._left = Left; }
        public LockFreeRbNode Right { get => _right; set => this._right = Right; }
        public LockFreeRbNode Parent { get => _parent; set => this._parent = Parent; }
        public bool IsRed { get => _isRed; set => this._isRed = IsRed; }

        public LockFreeRbNode()
        {
            _value = int.MinValue;
            _left = null;
            _right = null;
            _parent = null;
            _isRed = false;
            Flag = new AtomicBoolean(false);
        }

        public LockFreeRbNode(int value)
        {
            this._value = value;
            _left = new LockFreeRbNode();
            _right = new LockFreeRbNode();
            _parent = null;
            _isRed = true;
            Flag = new AtomicBoolean(false);
        }

        public int Height()
        {
            if (this == null)
            {
                return 0;
            }
            return 1 + Math.Max(Left.Height(), Right.Height());
        }

        public void DisplayNode()
        {
            Console.WriteLine(_value + " ");
        }
    }
}