using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Implementations
{
    internal sealed class BankFileStructureUpdateHandler : IBankFileStructureUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankFileStructureQueryService _bankFileStructureQueryService;
        public BankFileStructureUpdateHandler(
            IMapper mapper,
            IBankFileStructureQueryService bankFileStructureQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankFileStructureQueryService = bankFileStructureQueryService;
            _bankFileStructureQueryService.NotNull(nameof(_bankFileStructureQueryService));
        }

        public async Task Handle(BankFileStructureUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var bankFileStructure = await _bankFileStructureQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, bankFileStructure);
        }
    }
}
