using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Implementations
{
    internal sealed class BankGetSingleHandler : IBankGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankQueryService _bankQueryService;
        public BankGetSingleHandler(
            IMapper mapper,
            IBankQueryService bankQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankQueryService = bankQueryService;
            _bankQueryService.NotNull(nameof(_bankQueryService));
        }

        public async Task<BankGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var bank = await _bankQueryService.Get(id);
            return _mapper.Map<BankGetDto>(bank);
        }
    }
}
