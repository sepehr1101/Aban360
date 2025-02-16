using Aban360.Common.BaseEntities;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface ICaptchaQueryService
    {
        Task<Captcha> Get();
        Task<ICollection<Captcha>> GetAll();
        Task<ICollection<NumericDictionary>> GetDictionary();
        Task<Captcha> Get(int id);
    }
}