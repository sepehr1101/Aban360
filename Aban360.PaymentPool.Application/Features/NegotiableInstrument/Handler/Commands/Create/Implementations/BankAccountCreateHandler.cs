using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Commands.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Implementations
{
    internal sealed class BankAccountCreateHandler : IBankAccountCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountCommandService _bankAccountCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddhoc;
        public BankAccountCreateHandler(
            IMapper mapper,
            IBankAccountCommandService bankAccountCommandService,
            IZoneTitleAddhoc zoneTitleAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankAccountCommandService = bankAccountCommandService;
            _bankAccountCommandService.NotNull(nameof(_bankAccountCommandService));

            _zoneTitleAddhoc = zoneTitleAddhoc;
            _zoneTitleAddhoc.NotNull(nameof(_zoneTitleAddhoc));
        }

        public async Task Handle(BankAccountCreateDto createDto, CancellationToken cancellationToken)
        {
            var bankAccount = _mapper.Map<BankAccount>(createDto);
            bankAccount.ZoneTitle = await _zoneTitleAddhoc.Handle(createDto.ZoneId, cancellationToken);

            await _bankAccountCommandService.Add(bankAccount);
        }
    }
}
