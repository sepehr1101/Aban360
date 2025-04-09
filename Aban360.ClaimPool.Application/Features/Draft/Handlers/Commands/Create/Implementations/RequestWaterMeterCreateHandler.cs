using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestWaterMeterCreateHandler : IRequestWaterMeterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterCommandService _requestWaterMeterCommandService;
        public RequestWaterMeterCreateHandler(
            IMapper mapper,
            IRequestWaterMeterCommandService requestWaterMeterCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterCommandService = requestWaterMeterCommandService;
            _requestWaterMeterCommandService.NotNull(nameof(_requestWaterMeterCommandService));
        }

        public async Task Handle(WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestWaterMeter = _mapper.Map<RequestWaterMeter>(createDto);
            await _requestWaterMeterCommandService.Add(requestWaterMeter);
        }
    }
}
