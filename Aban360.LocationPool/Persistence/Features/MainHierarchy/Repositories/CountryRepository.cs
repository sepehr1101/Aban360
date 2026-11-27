using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Repositories
{
    internal sealed class CountryRepository : ICountryCommandService, ICountryQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Country> _country;
        public CountryRepository(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(Country));

            _country = _uow.Set<Country>();
            _country.NotNull(nameof(Country));
        }
        public async Task Add(Country country)
        {
           await _country.AddAsync(country);    
        }

        public async Task Remove(Country country)
        {
            _country.Remove(country);
        }

        public async Task<Country> Get(short Id)
        {
           return await _uow.FindOrThrowAsync<Country>(Id);
        }

        public async Task<ICollection<Country>> Get()
        {
            return await _country.ToListAsync();
        }        

    }
}
