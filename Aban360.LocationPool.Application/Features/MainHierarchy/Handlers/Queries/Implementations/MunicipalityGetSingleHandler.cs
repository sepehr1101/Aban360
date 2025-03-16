using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class MunicipalityGetSingleHandler : IMunicipalityGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityQueryService _municipalqueryService;
        public MunicipalityGetSingleHandler(
            IMapper mapper,
            IMunicipalityQueryService municipalqueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));
        }

        public async Task<MunicipalityGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            Municipality municipality = await _municipalqueryService.Get(id);
            return _mapper.Map<MunicipalityGetDto>(municipality);
        }
    }
}
