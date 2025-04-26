using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Implementations
{
    internal sealed class BankFileStructureDeleteHandler : IBankFileStructureDeleteHandler
    {
        private readonly IBankFileStructureCommandService _bankFileStructureCommandService;
        private readonly IBankFileStructureQueryService _bankFileStructureQueryService;
        public BankFileStructureDeleteHandler(
            IBankFileStructureCommandService bankFileStructureCommandService,
            IBankFileStructureQueryService bankFileStructureQueryService)
        {
            _bankFileStructureCommandService = bankFileStructureCommandService;
            _bankFileStructureCommandService.NotNull(nameof(_bankFileStructureCommandService));

            _bankFileStructureQueryService = bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task Handle(BankFileStructureDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var bankFileStructure = await _bankFileStructureQueryService.Get(deleteDto.Id);
            await _bankFileStructureCommandService.Remove(bankFileStructure);
        }
    }
}
