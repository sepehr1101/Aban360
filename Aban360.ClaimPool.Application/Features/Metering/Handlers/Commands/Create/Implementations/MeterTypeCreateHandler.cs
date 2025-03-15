using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class MeterTypeCreateHandler : IMeterTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeCommandService _meterTypeCommandService;
        public MeterTypeCreateHandler(
            IMapper mapper,
            IMeterTypeCommandService meterTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeCommandService = meterTypeCommandService;
            _meterTypeCommandService.NotNull(nameof(meterTypeCommandService));
        }

        public async Task Handle(MeterTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            MeterType meterType = _mapper.Map<MeterType>(createDto);
            await _meterTypeCommandService.Add(meterType);
        }
    }
}
