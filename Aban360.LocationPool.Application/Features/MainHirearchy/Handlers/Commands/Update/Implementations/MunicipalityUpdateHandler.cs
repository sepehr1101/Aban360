using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Implementations
{
    public class MunicipalityUpdateHandler : IMunicipalityUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMunicipalityQueryService _municipalqueryService;
        public MunicipalityUpdateHandler(
            IMapper mapper,
            IMunicipalityQueryService municipalqueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _municipalqueryService = municipalqueryService;
            _municipalqueryService.NotNull(nameof(municipalqueryService));
        }

        public async Task Handel(MunicipalityUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var municipality = await _municipalqueryService.Get(updateDto.Id);
            if (municipality == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, municipality);
        }
    }
}
