using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestWaterMeterTagCreateHandler : IRequestWaterMeterTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterTagCommandService _requestWaterMeterTagCommandService;
        public RequestWaterMeterTagCreateHandler(
            IMapper mapper,
            IRequestWaterMeterTagCommandService requestWaterMeterTagCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterTagCommandService = requestWaterMeterTagCommandService;
            _requestWaterMeterTagCommandService.NotNull(nameof(_requestWaterMeterTagCommandService));
        }

        public async Task Handle(WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var requestWaterMeterTag = _mapper.Map<RequestWaterMeterTag>(createDto);
            await _requestWaterMeterTagCommandService.Add(requestWaterMeterTag);
        }
    }
}
