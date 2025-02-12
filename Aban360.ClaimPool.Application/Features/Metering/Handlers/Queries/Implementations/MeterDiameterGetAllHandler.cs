using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    public class MeterDiameterGetAllHandler : IMeterDiameterGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        public MeterDiameterGetAllHandler(
            IMapper mapper,
            IMeterDiameterQueryService meterDiameterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

        }

        public async Task<ICollection<MeterDiameterGetDto>> Handle(CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterQueryService.Get();
            if (meterDiameter == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<MeterDiameterGetDto>>(meterDiameter);
        }
    }
}

