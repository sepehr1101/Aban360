using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class EstateCommandService : IEstateCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Estate> _estate;
        public EstateCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estate = _uow.Set<Estate>();
            _estate.NotNull(nameof(_estate));
        }

        public async Task Add(Estate estate)
        {
            await _estate.AddAsync(estate);
        }

        public async Task Remove(Estate estate)
        {
            _estate.Remove(estate);
        }
    }
}
