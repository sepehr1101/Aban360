using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Implementations
{
    public class MunicipalityCommandService : IMunicipalityCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Municipality> _municipality;
        public MunicipalityCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipality = _uow.Set<Municipality>();
            _municipality.NotNull(nameof(_municipality));
        }

        public async Task Add(Municipality municipality)
        {
            await _municipality.AddAsync(municipality);
        }

        public async Task Remove(Municipality municipality)
        {
            _municipality.Remove(municipality);
        }
    }

}
