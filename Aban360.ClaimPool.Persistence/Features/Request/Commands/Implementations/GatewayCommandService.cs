using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    internal sealed class GatewayCommandService : IGatewayCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Gateway> _geteway;
        public GatewayCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _geteway = _uow.Set<Gateway>();
            _geteway.NotNull(nameof(_geteway));
        }

        public async Task Add(Gateway geteway)
        {
            await _geteway.AddAsync(geteway);
        }

        public async Task Remove(Gateway geteway)
        {
            _geteway.Remove(geteway);
        }
    }
}