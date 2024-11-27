using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Aban360.UserPool.Persistence.Features.Auth.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class RoleCommandService : IRoleCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _roles;
        public RoleCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _roles = _uow.Set<Role>();
            _roles.NotNull(nameof(_roles));
        }
        public async Task Add(Role role)
        {
            await _roles.AddAsync(role);
        }
        public void Delete(Role role, string removeLogInfo)
        {
            role.ValidTo = DateTime.Now;
            role.RemoveLogInfo = removeLogInfo;
        }
    }
}