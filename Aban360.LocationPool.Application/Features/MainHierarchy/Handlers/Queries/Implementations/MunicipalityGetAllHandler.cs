using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    internal sealed class MunicipalityGetAllHandler : IMunicipalityGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityQueryService _municipalqueryService;
        public MunicipalityGetAllHandler(
            IMapper mapper,
            IMunicipalityQueryService municipalqueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));
        }

        public async Task<ICollection<MunicipalityGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<Municipality> municipality = await _municipalqueryService.Get();
            return _mapper.Map<ICollection<MunicipalityGetDto>>(municipality);
        }
    }
}
