using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class KitCodeDictionaryGetHandler : IKitCodeDictionaryGetHandler
    {
        public ICollection<NumericDictionary> Handle(CancellationToken cancellationToken)
        {
            var kitCodeDictionary = new List<NumericDictionary>()
            {
               new NumericDictionary(1,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
               new NumericDictionary(2,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
               new NumericDictionary(3,"بسته لوازم انشعاب قطر 1 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
               new NumericDictionary(4,"بسته لوازم انشعاب قطر 1,1/2 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
               new NumericDictionary(5,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
               new NumericDictionary(6,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
               new NumericDictionary(7,"بسته لوازم انشعاب قطر 1 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
               new NumericDictionary(8,"بسته لوازم انشعاب قطر 1,1/2 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
               new NumericDictionary(9,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
               new NumericDictionary(10,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
               new NumericDictionary(11,"بسته لوازم انشعاب قطر 1 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
               new NumericDictionary(12,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 2 واحد (تابلو محفظه دیواری)"),
               new NumericDictionary(13,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 3 واحد (تابلو محفظه دیواری)"),
               new NumericDictionary(14,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 4 واحد (تابلو محفظه دیواری)"),
               new NumericDictionary(15,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 2 واحد (محفظه زمینی)"),
               new NumericDictionary(16,"بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 3 واحد (محفظه زمینی)"),
               new NumericDictionary(17,"بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 4 واحد (محفظه زمینی)"),
            };

            return kitCodeDictionary;
        }
    }
}
