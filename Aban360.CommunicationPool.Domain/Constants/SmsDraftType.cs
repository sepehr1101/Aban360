using Aban360.Common.BaseEntities;
using Aban360.Common.Literals;

namespace Aban360.CommunicationPool.Domain.Constants
{
    public static class SmsDraftType
    {
        public static IEnumerable<NumericDictionary> Get()
        {
            return new List<NumericDictionary>
            {
                new NumericDictionary(CommonLiterals.MeterReadingBatchSmsDraftTypeId,"قبض دسته ای"),
                new NumericDictionary(CommonLiterals.TrackingSmsDraftTypeId ,"پیگیری درخواست"),
            };
        }
        public static string GetTitle(int id) => Get()?.Where(s => s.Id == id)?.Select(s => s.Title)?.Single() ?? string.Empty;
        public static NumericDictionary Get(int id) => Get()?.Where(s => s.Id == id).FirstOrDefault() ?? new NumericDictionary(0, string.Empty);
    }
}
