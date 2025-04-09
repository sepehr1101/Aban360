using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterUpdateHandler : IRequestWaterMeterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterQueryService _requestWaterMeterQueryService;
        public RequestWaterMeterUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterQueryService requestWaterMeterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterQueryService = requestWaterMeterQueryService;
            _requestWaterMeterQueryService.NotNull(nameof(_requestWaterMeterQueryService));
        }

        public async Task Handle(WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestWaterMeter = await _requestWaterMeterQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestWaterMeter);
        }
    }
}
