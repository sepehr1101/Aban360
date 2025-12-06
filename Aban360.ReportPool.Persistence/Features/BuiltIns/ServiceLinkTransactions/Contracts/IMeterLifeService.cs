using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts
{
    public interface IMeterLifeService
    {
        Task TruncateTable();
        Task<ReportOutput<MeterLifeHeaderOutputDto, MeterLifeDataOutputDto>> Get(MeterLifeInputDto input);
        Task<ReportOutput<MeterLifeSummaryHeaderOutputDto, MeterLifeSummaryDataOutputDto>> GetSummary(MeterLifeInputDto input);
        Task<IEnumerable<MeterLifeCalculationOutputDto>> GetFromClient();
        Task Insert(IEnumerable<MeterLifeCalculationOutputDto> input);
    }
}
