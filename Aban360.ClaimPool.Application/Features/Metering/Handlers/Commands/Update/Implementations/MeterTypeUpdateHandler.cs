using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    public class MeterTypeUpdateHandler : IMeterTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeQueryService _meterTypeQueryService;
        public MeterTypeUpdateHandler(IMapper mapper,
            IMeterTypeQueryService meterTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeQueryService = meterTypeQueryService;
            _meterTypeQueryService.NotNull(nameof(meterTypeQueryService));
        }

        public async Task Handle(MeterTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var meterType = await _meterTypeQueryService.Get(updateDto.Id);
            if (meterType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, meterType);
        }
    }
}
