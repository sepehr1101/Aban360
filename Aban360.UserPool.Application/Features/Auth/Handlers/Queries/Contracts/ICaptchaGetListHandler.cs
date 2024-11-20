using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaGetListHandler
    {
        Task<ICollection<CaptchaQueryDto>> Handle(CancellationToken cancellationToken);
    }
}