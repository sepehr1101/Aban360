using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts
{
    public interface IBranchSpecificationInfoGetHandler
    {
        Task<BranchSpecificationInfoDto> Handle(string billId, CancellationToken cancellationToken);
    }
}
