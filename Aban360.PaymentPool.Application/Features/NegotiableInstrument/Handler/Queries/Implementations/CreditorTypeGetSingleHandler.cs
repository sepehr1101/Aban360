using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class CreditorTypeGetSingleHandler : ICreditorTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICreditorTypeQueryService _creditorTypeQueryService;
        public CreditorTypeGetSingleHandler(
            IMapper mapper,
            ICreditorTypeQueryService creditorTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _creditorTypeQueryService = creditorTypeQueryService;
            _creditorTypeQueryService.NotNull(nameof(_creditorTypeQueryService));
        }

        public async Task<CreditorTypeGetDto> Handle(CreditorTypeEnum id, CancellationToken cancellationToken)
        {
            var creditorType = await _creditorTypeQueryService.Get(id);
            return _mapper.Map<CreditorTypeGetDto>(creditorType);
        }
    }
}
