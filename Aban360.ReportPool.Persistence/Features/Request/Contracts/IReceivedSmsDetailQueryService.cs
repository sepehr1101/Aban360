using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;

namespace Aban360.ReportPool.Persistence.Features.Request.Contracts
{
    public interface IReceivedSmsDetailQueryService
    {
        Task<IEnumerable<ReceivedSmsDataOutputDto>> Get(ReceivedSmsInputDto inputDto);
    }
}
