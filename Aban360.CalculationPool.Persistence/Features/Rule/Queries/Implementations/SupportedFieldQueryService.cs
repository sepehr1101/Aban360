using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.CalculationPool.Persistence.Features.Rule.Queries.Implementations
{
    internal sealed class SupportedFieldQueryService : ISupportedFieldQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SupportedField> _supportedField;
        public SupportedFieldQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _supportedField = _uow.Set<SupportedField>();
            _supportedField.NotNull(nameof(_supportedField));
        }

        public async Task<SupportedField> Get(short id)
        {
            return await _uow.FindOrThrowAsync<SupportedField>(id);
        }

        public async Task<ICollection<SupportedField>> Get()
        {
            return await _supportedField.ToListAsync();
        }
    }
}
