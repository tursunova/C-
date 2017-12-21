using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parallel_rbtree
{
    public interface RBTree
    {
        int? search(int? value);
        void insert(int? value);
    }
}
