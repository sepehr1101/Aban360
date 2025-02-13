using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Implementation
{
    public class CountryCommandService : ICountryCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Country> _country;
        public CountryCommandService(IUnitOfWork uow)
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
    }
}
