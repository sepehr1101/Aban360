using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Implementations
{
    internal sealed class PaymentMethodGetAllHandler : IPaymentMethodGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IPaymentMethodQueryService _paymentMethodQueryService;
        public PaymentMethodGetAllHandler(
            IMapper mapper,
            IPaymentMethodQueryService paymentMethodQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _paymentMethodQueryService = paymentMethodQueryService;
            _paymentMethodQueryService.NotNull(nameof(_paymentMethodQueryService));
        }

        public async Task<ICollection<PaymentMethodGetDto>> Handle(CancellationToken cancellationToken)
        {
            var bank = await _paymentMethodQueryService.Get();
            return _mapper.Map<ICollection<PaymentMethodGetDto>>(bank);
        }
    }
}
