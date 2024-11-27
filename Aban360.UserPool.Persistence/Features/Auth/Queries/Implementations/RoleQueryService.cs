using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    public class RoleQueryService : IRoleQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _roles;
        public RoleQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _roles = _uow.Set<Role>();
            _roles.NotNull(nameof(_roles));
        }

        public async Task<ICollection<Role>> Get()
        {
            return await _roles
                .Where(r => r.ValidTo != null)
                .ToListAsync();
        }
        public async Task<Role> Get(int id)
        {
            return await _roles.FindAsync(id);
        }
    }
}
