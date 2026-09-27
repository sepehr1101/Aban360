using Aban360.Common.BaseEntities;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface IWaterIncomeDiscountCauseGetHandler
    {
        IEnumerable<NumericDictionary> Handle(CancellationToken cancellationToken);
    }
}
