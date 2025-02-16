using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaGetSingleHandler
    {
        Task<CaptchaActiveDto> Handle(CancellationToken cancellationToken);
        Task<CaptchaSingleDto> Handle(int id, CancellationToken cancellationToken);
    }
}