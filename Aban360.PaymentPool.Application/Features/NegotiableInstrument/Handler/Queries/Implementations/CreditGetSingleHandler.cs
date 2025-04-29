using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class CreditGetSingleHandler : ICreditGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICreditQueryService _creditQueryService;
        public CreditGetSingleHandler(
            IMapper mapper,
            ICreditQueryService creditQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _creditQueryService = creditQueryService;
            _creditQueryService.NotNull(nameof(_creditQueryService));
        }

        public async Task<CreditGetDto> Handle(long id, CancellationToken cancellationToken)
        {
            var credit = await _creditQueryService.Get(id);
            return _mapper.Map<CreditGetDto>(credit);
        }
    }
}
