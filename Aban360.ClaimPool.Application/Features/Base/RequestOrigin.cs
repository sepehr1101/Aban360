using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Base
{
    public static class RequestOrigin
    {
        public static NumericDictionary? GetRequestOrigin(int id)
        {
            Dictionary<int, string> valueKeys = new()
            {
                { 1522,"1522" },
                { 2,"پرتال" },
                { 3,",USSD" },
                { 4,"اندروید"},
                { 5,"نظام "},
                { 6,"CRM"},
                { 8,"SMS"},
                { 10,"پنجره واحد هما" }
            };

            NumericDictionary? result = valueKeys
                .Where(r => r.Key == id)
                .Select(r => new NumericDictionary(r.Key, r.Value))
                .FirstOrDefault();

            return result;
        }
    }
}
