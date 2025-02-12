using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    public class MeterUseTypeUpdateHandler : IMeterUseTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
        public MeterUseTypeUpdateHandler(
            IMapper mapper,
            IMeterUseTypeQueryService meterUseTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeQueryService = meterUseTypeQueryService;
            _meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));
        }

        public async Task Handle(MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var meterUseType = await _meterUseTypeQueryService.Get(updateDto.Id);
            if (meterUseType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, meterUseType);
        }
    }
}
