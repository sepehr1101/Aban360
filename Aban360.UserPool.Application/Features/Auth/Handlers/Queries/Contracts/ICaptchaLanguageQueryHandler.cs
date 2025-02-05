using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Implementations
{
    public interface ICaptchaLanguageQueryHandler
    {
        Task<ICollection<CaptchaLanguageDto>> Handle(CancellationToken cancellationToken);
    }
}