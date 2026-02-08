namespace Aban360.ReportPool.Application.Features.Base
{
    public static class ReportAggregator
    {
        public static T AggregateGroup<T>(IEnumerable<T> items, string itemTitle)
            where T : new()
        {
            var aggregated = new T();
            var type = typeof(T);

            // Set ItemTitle (if it exists)
            var titleProp = type.GetProperty("ItemTitle");
            if (titleProp != null && titleProp.CanWrite)
                titleProp.SetValue(aggregated, itemTitle);

            // Collect numeric properties
            var props = type.GetProperties()
                .Where(p => p.CanWrite &&
                            (p.PropertyType == typeof(int) ||
                             p.PropertyType == typeof(long) ||
                             p.PropertyType == typeof(decimal) ||
                             p.PropertyType == typeof(double)));

            foreach (var prop in props)
            {
                double sum = items.Sum(x => Convert.ToDouble(prop.GetValue(x) ?? 0));

                if (prop.PropertyType == typeof(int))
                    prop.SetValue(aggregated, (int)sum);
                else if (prop.PropertyType == typeof(decimal))
                    prop.SetValue(aggregated, (decimal)sum);
                else if (prop.PropertyType == typeof(long))
                    prop.SetValue(aggregated, (long)sum);
                else if (prop.PropertyType == typeof(double))
                    prop.SetValue(aggregated, sum);                
            }

            return aggregated;
        }
    }

}
