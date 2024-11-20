using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface ICaptchaQueryService
    {
        Task<Captcha> Get();
        Task<ICollection<Captcha>> GetAll();
    }
}