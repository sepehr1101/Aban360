using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class BankFileStructureGetSingleHandler : IBankFileStructureGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankFileStructureQueryService _bankFileStructureQueryService;
        public BankFileStructureGetSingleHandler(
            IMapper mapper,
            IBankFileStructureQueryService bankFileStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankFileStructureQueryService = bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task<BankFileStructureGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var bankFileStructure = await _bankFileStructureQueryService.Get(id);
            return _mapper.Map<BankFileStructureGetDto>(bankFileStructure);
        }
    }
}
