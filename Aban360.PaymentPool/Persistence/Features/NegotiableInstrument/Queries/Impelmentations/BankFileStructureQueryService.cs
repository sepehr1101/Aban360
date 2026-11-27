using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Impelmentations
{
    internal sealed class BankFileStructureQueryService : IBankFileStructureQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankFileStructure> bankFileStructure;
        public BankFileStructureQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            bankFileStructure = _uow.Set<BankFileStructure>();
            bankFileStructure.NotNull(nameof(bankFileStructure));
        }

        public async Task<BankFileStructure> Get(short id)
        {
            return await _uow.FindOrThrowAsync<BankFileStructure>(id);
        }

        public async Task<ICollection<BankFileStructure>> Get()
        {
            return await bankFileStructure.ToListAsync();
        }
    }
}
