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
    }
}
