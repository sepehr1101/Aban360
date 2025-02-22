using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaGetSingleHandler
    {
        Task<CaptchaActiveDto> Handle(CancellationToken cancellationToken);
        Task<CaptchaQueryDto> Handle(int id, CancellationToken cancellationToken);
    }
}