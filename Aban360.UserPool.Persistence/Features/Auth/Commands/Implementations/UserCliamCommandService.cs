using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class UserCliamCommandService : IUserCliamCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserClaim> _userClaims;

        public UserCliamCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userClaims = _uow.Set<UserClaim>();
            _userClaims.NotNull(nameof(_userClaims));
        }
        public async Task Add(ICollection<UserClaim> userCliams)
        {
            await _userClaims.AddRangeAsync(userCliams);
        }
    }
}
