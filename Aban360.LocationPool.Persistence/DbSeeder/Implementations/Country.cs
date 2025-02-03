using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.LocationPool.Persistence.DbSeeder.Implementations
{
    public class CountryDataSeeder : IDataSeeder
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Country> _countries;
        public CountryDataSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _countries = _uow.Set<Country>();
        }
        public int Order { get; set; } = 1;

        public void SeedData()
        {
            var iran = new Country()
            {
                //Id = 1,
                Title = "IRAN"
            };
            _countries.Add(iran);
            _uow.SaveChanges();
        }
    }
}
