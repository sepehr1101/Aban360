using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaDisplayModeQueryHandler
    {
        Task<ICollection<CaptchaDisplayModeDto>> Handle(CancellationToken cancellationToken);
    }
}