using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts
{
    public interface IJudicialNoticeSetResultHandler
    {
        Task Handle(JudicalNoticeSetResultInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        ICollection<NumericDictionary> GetJudicialResults();
    }
}