using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts
{
    public interface IConnectDisconnectSetResultHandler
    {
        Task<ConnectDisconnectSetResultOutputDto> Handle(ServiceLinkConnectionInput inputDto, bool isConnect, IAppUser appUser, CancellationToken cancellationToken);
        ICollection<NumericDictionary> GetDisconnectResults();
    }
}
