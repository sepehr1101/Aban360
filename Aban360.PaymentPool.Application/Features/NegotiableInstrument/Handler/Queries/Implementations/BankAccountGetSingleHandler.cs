using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class BankAccountGetSingleHandler : IBankAccountGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountQueryService _bankAccountQueryService;
        public BankAccountGetSingleHandler(
            IMapper mapper,
            IBankAccountQueryService bankAccountQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankAccountQueryService = bankAccountQueryService;
            _bankAccountQueryService.NotNull(nameof(_bankAccountQueryService));
        }

        public async Task<BankAccountGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var BankAccount = await _bankAccountQueryService.Get(id);
            return _mapper.Map<BankAccountGetDto>(BankAccount);
        }
    }
}
