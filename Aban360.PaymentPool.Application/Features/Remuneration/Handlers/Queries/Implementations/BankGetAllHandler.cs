using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Implementations
{
    internal sealed class BankGetAllHandler : IBankGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankQueryService _bankQueryService;
        public BankGetAllHandler(
            IMapper mapper,
            IBankQueryService bankQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankQueryService = bankQueryService;
            _bankQueryService.NotNull(nameof(_bankQueryService));
        }

        public async Task<ICollection<BankGetDto>> Handle(CancellationToken cancellationToken)
        {
            var bank = await _bankQueryService.Get();
            return _mapper.Map<ICollection<BankGetDto>>(bank);
        }
    }
}
