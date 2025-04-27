using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Implementations
{
    internal sealed class CreditUpdateHandler : ICreditUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICreditQueryService _creditQueryService;
        public CreditUpdateHandler(
            IMapper mapper,
            ICreditQueryService creditQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _creditQueryService = creditQueryService;
            _creditQueryService.NotNull(nameof(_creditQueryService));
        }

        public async Task Handle(CreditUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var credit = await _creditQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, credit);
        }
    }
}
