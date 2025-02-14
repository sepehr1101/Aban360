using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    public class SiphonMaterialGetAllHandler : ISiphonMaterialGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonMaterialQueryService _queryService;
        public SiphonMaterialGetAllHandler(
            IMapper mapper,
            ISiphonMaterialQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<SiphonMaterialGetDto>> Handle(CancellationToken cancellationToken)
        {
            var siphonMaterial = await _queryService.Get();
            if (siphonMaterial == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<SiphonMaterialGetDto>>(siphonMaterial);
        }
    }
}
