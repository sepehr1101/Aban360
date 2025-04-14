using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    internal sealed class SupportedOperatorQueryService : ISupportedOperatorQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SupportedOperator> _supportedOperator;
        public SupportedOperatorQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _supportedOperator = _uow.Set<SupportedOperator>();
            _supportedOperator.NotNull(nameof(_supportedOperator));
        }

        public async Task<SupportedOperator> Get(short id)
        {
            return await _uow.FindOrThrowAsync<SupportedOperator>(id);
        }

        public async Task<ICollection<SupportedOperator>> Get()
        {
            return await _supportedOperator.ToListAsync();
        }
    }
}
