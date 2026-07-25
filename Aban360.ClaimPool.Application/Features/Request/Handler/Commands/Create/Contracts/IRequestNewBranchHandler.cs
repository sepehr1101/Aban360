using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface IRequestNewBranchHandler
    {
        Task<(MoshtrakCreateDto, SetAssessmentTimeDataOutputDto?, Guid)> Handle(RequestNewBranchInputDto inputDto, int userName, CancellationToken cancellationToken);
    }
}
