using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts
{
    public interface IAbonmanCreateHandler
    {
        Task Handle(AbonmanCreateDto createDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
