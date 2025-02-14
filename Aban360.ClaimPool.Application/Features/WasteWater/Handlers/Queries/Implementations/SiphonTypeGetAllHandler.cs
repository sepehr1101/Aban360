using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class SiphonTypeGetAllHandler : ISiphonTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonTypeQueryService _queryService;
        public SiphonTypeGetAllHandler(
            IMapper mapper,
            ISiphonTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<SiphonTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var siphonType = await _queryService.Get();
            if (siphonType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<SiphonTypeGetDto>>(siphonType);
        }
    }
}
