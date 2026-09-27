using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class WaterIncomeDiscountCauseGetHandler : IWaterIncomeDiscountCauseGetHandler
    {
        public IEnumerable<NumericDictionary> Handle(CancellationToken cancellationToken)
        {
            return new List<NumericDictionary>() 
            { 
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.Madrese ,ReportLiterals.Madrese ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.KomiteEmdad ,ReportLiterals.KomiteEmdad ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.Behzisti ,ReportLiterals.Behzisti ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.GolzarShohada ,ReportLiterals.GolzarShohada ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.KhaneAlem ,ReportLiterals.KhaneAlem ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.Masjed ,ReportLiterals.Masjed ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.Hoseiniye ,ReportLiterals.Hoseiniye ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.DarolGhoran ,ReportLiterals.DarolGhoran ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.MadaresOlomDini ,ReportLiterals.MadaresOlomDini ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.AmakenMazhabiVaEmamZadeh ,ReportLiterals.AmakenMazhabiVaEmamZadeh ),
                new NumericDictionary((int)WaterIncomeDiscountCauseEnum.TajmiTakhfif ,ReportLiterals.TajmiTakhfif ),
            };
        }
    }
}
