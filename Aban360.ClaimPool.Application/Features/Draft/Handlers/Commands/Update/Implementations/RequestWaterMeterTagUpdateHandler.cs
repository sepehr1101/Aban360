using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterTagUpdateHandler : IRequestWaterMeterTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterTagQueryService _requestWaterMeterTagQueryService;
        public RequestWaterMeterTagUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterTagQueryService requestWaterMeterTagQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterTagQueryService = requestWaterMeterTagQueryService;
            _requestWaterMeterTagQueryService.NotNull(nameof(_requestWaterMeterTagQueryService));
        }

        public async Task Handle(WaterMeterTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterTag = await _requestWaterMeterTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestWaterMeterTag);
        }
    }
}
