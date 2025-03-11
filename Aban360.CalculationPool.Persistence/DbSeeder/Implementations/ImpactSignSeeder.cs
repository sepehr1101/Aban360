using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.DbSeeder.Implementations
{
    public class ImpactSignSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfWork _uow;
        private readonly DbSet<ImpactSign> _impactSigns;
        public ImpactSignSeeder(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _impactSigns = _uow.Set<ImpactSign>();
            _impactSigns.NotNull(nameof(_impactSigns));
        }

        public void SeedData()
        {
            if (_impactSigns.Any())
            {
                return;
            }

            var ImpactSigns = GetImpactSigns();
            _impactSigns.AddRange(ImpactSigns);
            _uow.SaveChanges();
        }
        private ICollection<ImpactSign> GetImpactSigns()
        {
            ICollection<ImpactSign> ImpactSigns = new List<ImpactSign>()
            {
                new ImpactSign(){Id=ImpactSignEnum.Positive,Multiplier=+1,Title="مثبت"},
                new ImpactSign(){Id=ImpactSignEnum.Negative,Multiplier=-1,Title="منفی"},
            };

            return ImpactSigns;
        }
    }
}
