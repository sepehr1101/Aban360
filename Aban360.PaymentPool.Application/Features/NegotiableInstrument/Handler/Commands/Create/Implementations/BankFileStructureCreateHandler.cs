using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Implementations
{
    internal sealed class BankFileStructureCreateHandler : IBankFileStructureCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankFileStructureCommandService _bankFileStructureCommandService;
        public BankFileStructureCreateHandler(
            IMapper mapper,
            IBankFileStructureCommandService bankFileStructureCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankFileStructureCommandService = bankFileStructureCommandService;
            _bankFileStructureCommandService.NotNull(nameof(_bankFileStructureCommandService));
        }

        public async Task Handle(BankFileStructureCreateDto createDto, CancellationToken cancellationToken)
        {
            var bankFileStructure = _mapper.Map<BankFileStructure>(createDto);
            await _bankFileStructureCommandService.Add(bankFileStructure);
        }
    }
}
