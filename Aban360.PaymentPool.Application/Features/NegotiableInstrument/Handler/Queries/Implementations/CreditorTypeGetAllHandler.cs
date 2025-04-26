using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class CreditorTypeGetAllHandler : ICreditorTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICreditorTypeQueryService _creditorTypeQueryService;
        public CreditorTypeGetAllHandler(
            IMapper mapper,
            ICreditorTypeQueryService creditorTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _creditorTypeQueryService = creditorTypeQueryService;
            _creditorTypeQueryService.NotNull(nameof(_creditorTypeQueryService));
        }

        public async Task<ICollection<CreditorTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var creditorType = await _creditorTypeQueryService.Get();
            return _mapper.Map<ICollection<CreditorTypeGetDto>>(creditorType);
        }
    }
}
