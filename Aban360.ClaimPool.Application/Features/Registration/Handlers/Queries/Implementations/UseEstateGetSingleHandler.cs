using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Registration.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Implementations
{
    public class UseEstateGetSingleHandler : IUseEstateGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUseStateQueryService _useStateQueryService;
        public UseEstateGetSingleHandler(
            IMapper mapper,
            IUseStateQueryService useStateQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _useStateQueryService = useStateQueryService;
            _useStateQueryService.NotNull(nameof(useStateQueryService));
        }

        public async Task<UseStateGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var useState = await _useStateQueryService.Get(id);
            if (useState == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<UseStateGetDto>(useState);
        }
    }
}
