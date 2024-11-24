using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts
{
    public interface ICaptchaGetListHandler
    {
        Task<ICollection<CaptchaListQueryDto>> Handle(CancellationToken cancellationToken);
    }
}