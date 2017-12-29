namespace parallel_rbtree
{
    public interface IRbTree
    {
        int? Search(int? value);
        void Insert(int? value);
    }
}