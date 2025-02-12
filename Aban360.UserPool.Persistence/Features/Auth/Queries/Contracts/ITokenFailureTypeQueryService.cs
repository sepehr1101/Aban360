using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts
{
    public interface ITokenFailureTypeQueryService
    {
        Task<TokenFailureType> Get(TokenFailureTypeEnum id);
    }
}