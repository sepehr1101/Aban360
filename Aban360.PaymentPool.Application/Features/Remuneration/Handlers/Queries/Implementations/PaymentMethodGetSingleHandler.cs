using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Implementations
{
    internal sealed class PaymentMethodGetSingleHandler : IPaymentMethodGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IPaymentMethodQueryService _paymentMethodQueryService;
        public PaymentMethodGetSingleHandler(
            IMapper mapper,
            IPaymentMethodQueryService paymentMethodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _paymentMethodQueryService = paymentMethodQueryService;
            _paymentMethodQueryService.NotNull(nameof(_paymentMethodQueryService));
        }

        public async Task<PaymentMethodGetDto> Handle(PaymentMethodEnum id, CancellationToken cancellationToken)
        {
            var bank = await _paymentMethodQueryService.Get(id);
            return _mapper.Map<PaymentMethodGetDto>(bank);
        }
    }
}
