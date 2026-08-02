using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IUsageGroup1DuplicateInsertHandler
    {
        Task<UsageGroup1DuplicateInsertOutputDto> Handle(UsageGroup1DuplicateInsetInputDto inputDto, CancellationToken cancellationToken);
    }
}
