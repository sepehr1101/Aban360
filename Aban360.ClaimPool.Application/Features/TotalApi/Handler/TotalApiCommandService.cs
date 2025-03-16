using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.TotalApi.Handler
{
    internal sealed class TotalApiCommandService : ITotalApiCommandService
    {
        private readonly IMapper _mapper;
        private readonly IEstateCommandService _estateCommandService;
        private readonly IWaterMeterCommandService _waterMeterCommandService;
        private readonly ISiphonCommandService _siphonCommandService;
        private readonly IIndividualCommandService _individualCommandService;
        public TotalApiCommandService(
            IMapper mapper,
            IEstateCommandService estateCommandService,
            IWaterMeterCommandService waterMeterCommandService,
            ISiphonCommandService siphonCommandService,
            IIndividualCommandService individualCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _estateCommandService = estateCommandService;
            _estateCommandService.NotNull(nameof(estateCommandService));

            _waterMeterCommandService = waterMeterCommandService;
            _waterMeterCommandService.NotNull(nameof(waterMeterCommandService));

            _siphonCommandService = siphonCommandService;
            _siphonCommandService.NotNull(nameof(siphonCommandService));

            _individualCommandService = individualCommandService;
            _individualCommandService.NotNull(nameof(individualCommandService));
        }

        public async Task Handle(TotalApiCreateDto createDto, CancellationToken cancellationToken)
        {
            Estate estate = _mapper.Map<Estate>(createDto.Estate);
            WaterMeter waterMeter = _mapper.Map<WaterMeter>(createDto.WaterMeter);
            ICollection<Siphon> siphons = _mapper.Map<ICollection<Siphon>>(createDto.siphons);
            ICollection<Individual> individuals = _mapper.Map<ICollection<Individual>>(createDto.individuals);

            await _estateCommandService.Add(estate);
            await _waterMeterCommandService.Add(waterMeter);
            await _siphonCommandService.Add(siphons);
            await _individualCommandService.Add(individuals);

        }
    }
}
