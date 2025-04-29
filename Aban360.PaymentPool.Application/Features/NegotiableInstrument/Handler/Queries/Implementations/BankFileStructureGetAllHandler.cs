using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class BankFileStructureGetAllHandler : IBankFileStructureGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankFileStructureQueryService _bankFileStructureQueryService;
        public BankFileStructureGetAllHandler(
            IMapper mapper,
            IBankFileStructureQueryService bankFileStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankFileStructureQueryService = bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task<ICollection<BankFileStructureGetDto>> Handle(CancellationToken cancellationToken)
        {
            var bankFileStructure = await _bankFileStructureQueryService.Get();
            return _mapper.Map<ICollection<BankFileStructureGetDto>>(bankFileStructure);
        }
    }
}
