using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Features.NegotiableInstrument.Queries.Contracts;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Implementations
{
    internal sealed class AccountTypeGetSingleHandler : IAccountTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IAccountTypeQueryService _accountTypeQueryService;
        public AccountTypeGetSingleHandler(
            IMapper mapper,
            IAccountTypeQueryService accountTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _accountTypeQueryService = accountTypeQueryService;
            _accountTypeQueryService.NotNull(nameof(_accountTypeQueryService));
        }

        public async Task<AccountTypeGetDto> Handle(AccountTypeEnum id, CancellationToken cancellationToken)
        {
            var accountType = await _accountTypeQueryService.Get(id);
            return _mapper.Map<AccountTypeGetDto>(accountType);
        }
    }
}
