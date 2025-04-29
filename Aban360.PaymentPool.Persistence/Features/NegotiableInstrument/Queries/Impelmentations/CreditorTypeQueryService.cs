using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class CreditorTypeQueryService : ICreditorTypeQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CreditorType> creaditorType;
        public CreditorTypeQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            creaditorType = _uow.Set<CreditorType>();
            creaditorType.NotNull(nameof(creaditorType));
        }

        public async Task<CreditorType> Get(CreditorTypeEnum id)
        {
            return await _uow.FindOrThrowAsync<CreditorType>(id);
        }

        public async Task<ICollection<CreditorType>> Get()
        {
            return await creaditorType.ToListAsync();
        }
    }
}
