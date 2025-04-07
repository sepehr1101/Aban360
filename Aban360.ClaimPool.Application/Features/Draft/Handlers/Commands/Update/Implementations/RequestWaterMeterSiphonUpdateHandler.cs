using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterSiphonUpdateHandler : IRequestWaterMeterSiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterSiphonQueryService _requestWaterMeterSiphonQueryService;
        public RequestWaterMeterSiphonUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterSiphonQueryService requestWaterMeterSiphonQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterSiphonQueryService = requestWaterMeterSiphonQueryService;
            _requestWaterMeterSiphonQueryService.NotNull(nameof(_requestWaterMeterSiphonQueryService));
        }

        public async Task Handle(WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterSiphon = await _requestWaterMeterSiphonQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestWaterMeterSiphon);
        }
    }
}
