using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaGetSingleHandler
    {
        Task<CaptchaSingleQueryDto> Handle(CancellationToken cancellationToken);
    }
}