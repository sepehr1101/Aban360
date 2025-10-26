namespace Aban360.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items) 
                action(item);
        }

        public static bool HasValue<T>(this IEnumerable<T> items)
            => items?.Any() is true;

        public static int CountValue<T>(this IEnumerable<T> items)
            => items.HasValue() ? items.Count() : 0;

        public static int MaxValue(this IEnumerable<int> items)
            => items.HasValue() ? items.Max() : 0;

        public static int MinValue(this IEnumerable<int> items)
            => items.HasValue() ? items.Min() : 0;
    }
}
