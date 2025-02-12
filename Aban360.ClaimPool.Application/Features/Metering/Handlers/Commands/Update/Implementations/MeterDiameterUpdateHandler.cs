using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    public class MeterDiameterUpdateHandler : IMeterDiameterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        public MeterDiameterUpdateHandler(
            IMapper mapper,
            IMeterDiameterQueryService meterDiameterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

        }

        public async Task Handle(MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterQueryService.Get(updateDto.Id);
            if (meterDiameter == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto.Id, meterDiameter);
        }
    }
}

