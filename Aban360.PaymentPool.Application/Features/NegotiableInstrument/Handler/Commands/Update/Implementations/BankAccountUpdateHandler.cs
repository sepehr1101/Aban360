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
        private readonly IRegionTitleAddhoc _regionTitleAddhoc;

        public BankAccountUpdateHandler(
            IMapper mapper,
            IBankAccountQueryService bankAccountQueryService,
            IRegionTitleAddhoc regionTitleAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankAccountQueryService = bankAccountQueryService;
            _bankAccountQueryService.NotNull(nameof(_bankAccountQueryService));

            _regionTitleAddhoc = regionTitleAddhoc;
            _regionTitleAddhoc.NotNull(nameof(_regionTitleAddhoc));
        }

        public async Task Handle(BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountQueryService.Get(updateDto.Id);
            bankAccount.RegionTitle = await _regionTitleAddhoc.Handle(updateDto.RegionId, cancellationToken);

            _mapper.Map(updateDto, bankAccount);
        }
    }
}
