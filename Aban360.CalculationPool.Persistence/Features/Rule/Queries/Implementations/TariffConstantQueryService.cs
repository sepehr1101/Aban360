using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    internal sealed class TariffConstantQueryService : ITariffConstantQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<TariffConstant> _tariffConstant;
        public TariffConstantQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _tariffConstant = _uow.Set<TariffConstant>();
            _tariffConstant.NotNull(nameof(_tariffConstant));
        }

        public async Task<TariffConstant> Get(short id)
        {
            return await _uow.FindOrThrowAsync<TariffConstant>(id);
        }

        public async Task<ICollection<TariffConstant>> Get()
        {
            return await _tariffConstant.ToListAsync();
        }
        public async Task<ICollection<TariffConstant>> Get(string @from, string @to)
        {
            return await _tariffConstant
                .AsNoTracking()
                .Where(tariff =>
                       tariff.FromDateJalali.CompareTo(to) <= 0 &&
                       tariff.ToDateJalali.CompareTo(from) >= 0)
                .ToListAsync();
        }
        public async Task<ICollection<StringDictionary>> GetDictionary()
        {
            return await _tariffConstant
                .AsNoTracking()
                .GroupBy(constant => constant.Key)
                .Select(c => new StringDictionary() { Id = c.Key, Title = c.First().Value })
                .ToListAsync();

        }
    }
}
