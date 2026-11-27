using Aban360.Common.ApplicationUser;
using Aban360.TaxPool.Domain.Features.MaaherSTP.Dto.SendDto;

namespace Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Commands.Contracts
{
    public interface INewListCreateHandler
    {
        Task<int> Handle(NewListCreateDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
