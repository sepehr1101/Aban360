using Aban360.Common.BaseEntities;

namespace Aban360.CommunicationPool.Domain.Constants
{
    public static class SmsManagerType
    {
        public static IEnumerable<NumericDictionary> Get()
        {
            return new List<NumericDictionary>
            {
                new NumericDictionary(1,"قبض دسته ای"),
                new NumericDictionary(2,"پیگیری درخواست"),
            };
        }
        public static string Get(int id) => Get()?.Where(s => s.Id == id)?.Select(s => s.Title)?.Single() ?? string.Empty;
    }
}
