using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class UserClaimQueryService : IUserClaimQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserClaim> _userClaims;
        public UserClaimQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userClaims = _uow.Set<UserClaim>();
            _userClaims.NotNull(nameof(_userClaims));
        }

        public IQueryable<UserClaim> GetQuery()
        {
            return _userClaims.AsQueryable();
        }
        public async Task<ICollection<UserClaim>> Get(Guid userId)
        {
            return await _userClaims
                .Where(uc => 
                    uc.UserId == userId &&
                    uc.ValidTo == null)
                .ToListAsync();
        }
        public async Task<ICollection<UserClaim>> Get(Guid userId, ClaimType claimType)
        {
            return await _userClaims
                .Where(userClaim => 
                    userClaim.UserId == userId &&
                    userClaim.ClaimTypeId == claimType)
                .ToListAsync();
        }
    }
}