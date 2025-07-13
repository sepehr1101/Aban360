using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts
{
    public interface IReadingListDetailQueryService
    {
        Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListDetailDataOutputDto>> GetInfo(ReadingListInputDto input);
    }
}
