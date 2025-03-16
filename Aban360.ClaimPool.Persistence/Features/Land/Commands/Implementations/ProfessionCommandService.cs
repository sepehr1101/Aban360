using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class ProfessionCommandService : IProfessionCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Profession> _profession;
        public ProfessionCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _profession = _uow.Set<Profession>();
            _profession.NotNull(nameof(_profession));
        }

        public async Task Add(Profession profession)
        {
            await _profession.AddAsync(profession);
        }

        public async Task Remove(Profession profession)
        {
            _profession.Remove(profession);
        }
    }
}
