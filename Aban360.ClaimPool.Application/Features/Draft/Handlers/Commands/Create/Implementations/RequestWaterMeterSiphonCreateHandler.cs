using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestWaterMeterSiphonCreateHandler : IRequestWaterMeterSiphonCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterSiphonCommandService _requestWaterMeterSiphonCommandService;
        public RequestWaterMeterSiphonCreateHandler(
            IMapper mapper,
            IRequestWaterMeterSiphonCommandService requestWaterMeterSiphonCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterSiphonCommandService = requestWaterMeterSiphonCommandService;
            _requestWaterMeterSiphonCommandService.NotNull(nameof(_requestWaterMeterSiphonCommandService));
        }

        public async Task Handle(WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterSiphon = _mapper.Map<RequestWaterMeterSiphon>(createDto);
            await _requestWaterMeterSiphonCommandService.Add(requestWaterMeterSiphon);
        }
    }
}
