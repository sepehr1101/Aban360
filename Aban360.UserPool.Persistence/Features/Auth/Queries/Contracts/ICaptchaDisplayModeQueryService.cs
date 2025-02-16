using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface ICaptchaDisplayModeQueryService
    {
        Task<ICollection<CaptchaDisplayMode>> Get();
        Task<CaptchaDisplayMode> Get(short id);
    }
}