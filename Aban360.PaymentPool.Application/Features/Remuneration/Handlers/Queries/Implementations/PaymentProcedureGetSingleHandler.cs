using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Implementations
{
    internal sealed class PaymentProcedureGetSingleHandler : IPaymentProcedureGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IPaymentProcedureQueryService _bankQueryService;
        public PaymentProcedureGetSingleHandler(
            IMapper mapper,
            IPaymentProcedureQueryService bankQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankQueryService = bankQueryService;
            _bankQueryService.NotNull(nameof(_bankQueryService));
        }

        public async Task<PaymentProcedureGetDto> Handle(PaymentProcedureEnum id, CancellationToken cancellationToken)
        {
            var bank = await _bankQueryService.Get(id);
            return _mapper.Map<PaymentProcedureGetDto>(bank);
        }
    }
}
