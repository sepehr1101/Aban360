using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts
{
    public interface IUserWorkdayDeleteHandler
    {
        Task Handle(UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
