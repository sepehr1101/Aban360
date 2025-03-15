using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class MeterTypeGetAllHandler : IMeterTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeQueryService _meterTypeQueryService;
        public MeterTypeGetAllHandler(IMapper mapper,
            IMeterTypeQueryService meterTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeQueryService = meterTypeQueryService;
            _meterTypeQueryService.NotNull(nameof(meterTypeQueryService));
        }

        public async Task<ICollection<MeterTypeGetDto>> Handle( CancellationToken cancellationToken)
        {
            ICollection<MeterType> meterType = await _meterTypeQueryService.Get();
            if (meterType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<MeterTypeGetDto>>(meterType);
        }
    }
}
