using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Implementations
{
    public class UseStateGetAllHandler : IUseStateGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateQueryService _useStateQueryService;
        public UseStateGetAllHandler(
            IMapper mapper,
            IUseStateQueryService useStateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateQueryService = useStateQueryService;
            _useStateQueryService.NotNull(nameof(useStateQueryService));
        }

        public async Task<ICollection<UseStateGetDto>> Handle(CancellationToken cancellationToken)
        {
            var useState = await _useStateQueryService.Get();
            if (useState == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<UseStateGetDto>>(useState);
        }
    }
}
