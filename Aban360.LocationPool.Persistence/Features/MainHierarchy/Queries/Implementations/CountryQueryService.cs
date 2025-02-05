using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Implementations
{
    public class CountryQueryService : ICountryQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Country> _country;
        public CountryQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _country = _uow.Set<Country>();
            _country.NotNull(nameof(_country));
        }

        public async Task<Country> Get(short Id)
        {
           return  _uow.FindOrThrow<Country>(Id);
        }

        public async Task<ICollection<Country>> Get()
        {
            return await _country.ToListAsync();
        }
    }
}
