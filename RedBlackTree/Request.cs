using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    public class Request
    {
        public int type;
        public int key;
        public int value = 0;
        public Request(int op, int k, int val)
        {
            this.type = op;
            this.key = k;
            this.value = val;
        }
    }
}
