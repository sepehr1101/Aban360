using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class MeterUseTypeCreateHandler : IMeterUseTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeCommandService _meterUseTypeCommandService;
        public MeterUseTypeCreateHandler(
            IMapper mapper,
            IMeterUseTypeCommandService meterUseTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeCommandService = meterUseTypeCommandService;
            _meterUseTypeCommandService.NotNull(nameof(meterUseTypeCommandService));
        }

        public async Task Handle(MeterUseTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            MeterUseType meterUseType = _mapper.Map<MeterUseType>(createDto);
            await _meterUseTypeCommandService.Add(meterUseType);
        }
    }
}
