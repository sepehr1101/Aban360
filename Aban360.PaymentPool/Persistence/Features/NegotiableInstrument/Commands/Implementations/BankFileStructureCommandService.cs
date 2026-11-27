using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Implementations
{
    internal sealed class BankFileStructureCommandService : IBankFileStructureCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BankFileStructure> _bankFileStructure;
        public BankFileStructureCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _bankFileStructure = _uow.Set<BankFileStructure>();
            _bankFileStructure.NotNull(nameof(_bankFileStructure));
        }

        public async Task Add(BankFileStructure bankFileStructure)
        {
            await _bankFileStructure.AddAsync(bankFileStructure);
        }

        public async Task Remove(BankFileStructure bankFileStructure)
        {
            _bankFileStructure.Remove(bankFileStructure);
        }
    }
}
