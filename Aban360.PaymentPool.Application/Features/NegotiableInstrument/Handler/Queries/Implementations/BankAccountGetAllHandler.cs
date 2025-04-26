using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;
using System.Threading;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class BankAccountGetAllHandler : IBankAccountGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountQueryService _bankAccountQueryService;
        public BankAccountGetAllHandler(
            IMapper mapper,
            IBankAccountQueryService bankAccountQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _bankAccountQueryService = bankAccountQueryService;
            _bankAccountQueryService.NotNull(nameof(_bankAccountQueryService));
        }

        public async Task<ICollection<BankAccountGetDto>> Handle(CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountQueryService.Get();
            return _mapper.Map<ICollection<BankAccountGetDto>>(bankAccount);
        }
    }
}
