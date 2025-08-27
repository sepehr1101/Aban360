using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IReadingChecklistQueryService
    {
        Task<ReportOutput<ReadingChecklistHeaderOutputDto, ReadingChecklistDataOutputDto>> Get(ReadingChecklistInputDto input);
    }
}
