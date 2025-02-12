using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal class TokenFailureTypeQueryService : ITokenFailureTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TokenFailureType> _tokenFailureTypes;
        public TokenFailureTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tokenFailureTypes = _uow.Set<TokenFailureType>();
            _tokenFailureTypes.NotNull(nameof(_tokenFailureTypes));
        }
        public async Task<TokenFailureType> Get(TokenFailureTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<TokenFailureType>(id);
        }
    }
}
