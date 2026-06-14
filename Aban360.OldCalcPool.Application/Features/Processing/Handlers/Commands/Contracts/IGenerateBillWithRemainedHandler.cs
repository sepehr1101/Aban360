using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IGenerateBillWithRemainedHandler
    {
        Task<BillIssueRemainedOutputDto> Handle(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
