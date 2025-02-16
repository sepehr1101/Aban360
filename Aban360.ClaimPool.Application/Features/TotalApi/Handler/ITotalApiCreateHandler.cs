using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
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
    public interface ITotalApiCommandService
    {
        Task Handle(TotalApiCreateDto createDto, CancellationToken cancellationToken);
    }
    public class TotalApiCommandService : ITotalApiCommandService
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
            var estate=_mapper.Map<Estate>(createDto.Estate);
            var waterMeter=_mapper.Map<WaterMeter>(createDto.WaterMeter);
            var siphons=_mapper.Map<ICollection<Siphon>>(createDto.siphons);
            var individuals=_mapper.Map<ICollection<Individual>>(createDto.individuals);

            await _estateCommandService.Add(estate);
            await _waterMeterCommandService.Add(waterMeter);
            await _siphonCommandService.Add(siphons);
            await _individualCommandService.Add(individuals);

        }
    }
}
