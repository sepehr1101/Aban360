using Aban360.ClaimPool.Domain.Features.Request.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public class GetewayCommandService : IGetewayCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Geteway> _geteway;
        public GetewayCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _geteway = _uow.Set<Geteway>();
            _geteway.NotNull(nameof(_geteway));
        }

        public async Task Add(Geteway geteway)
        {
            await _geteway.AddAsync(geteway);
        }

        public async Task Remove(Geteway geteway)
        {
            _geteway.Remove(geteway);
        }
    }
}