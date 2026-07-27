using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts
{
    public interface IBillIdTagRemoveByTagIdsHandler
    {
        Task<BillIdTagRemoveByTagIdsOutputDto> Handle(BillIdTagRemoveByTagIdsInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
