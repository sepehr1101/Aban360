using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Implementations
{
    internal sealed class BankAccountUpdateHandler : IBankAccountUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountQueryService _bankAccountQueryService;
        private readonly IZoneTitleAddhoc _zoneTitleAddhoc;

        public BankAccountUpdateHandler(
            IMapper mapper,
            IBankAccountQueryService bankAccountQueryService,
            IZoneTitleAddhoc zoneTitleAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankAccountQueryService = bankAccountQueryService;
            _bankAccountQueryService.NotNull(nameof(_bankAccountQueryService));

            _zoneTitleAddhoc = zoneTitleAddhoc;
            _zoneTitleAddhoc.NotNull(nameof(_zoneTitleAddhoc));
        }

        public async Task Handle(BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountQueryService.Get(updateDto.Id);
            bankAccount.ZoneTitle = await _zoneTitleAddhoc.Handle(updateDto.ZoneId, cancellationToken);

            _mapper.Map(updateDto, bankAccount);
        }
    }
}
