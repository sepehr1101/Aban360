using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    /* public interface IWaterMeterTagDefinitionCreateHandler
     {
         Task Handle(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken);
     }
     public class WaterMeterTagDefinitionCreateHandler : IWaterMeterTagDefinitionCreateHandler
     {
         private readonly IMapper _mapper;
         private readonly IWaterMeterTagDefinitionCommandService _commandService;
         private readonly IWaterMeterTagDefinitionQueryService _queryService;
         public WaterMeterTagDefinitionCreateHandler(
             IMapper mapper,
             IWaterMeterTagDefinitionCommandService commandService,
             IWaterMeterTagDefinitionQueryService queryService)
         {
             _mapper = mapper;
             _mapper.NotNull(nameof(mapper));

             _commandService = commandService;
             _commandService.NotNull(nameof(commandService));

             _queryService = queryService;
             _queryService.NotNull(nameof(queryService));
         }

         public async Task Handle(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
         {
             var waterMeterTagDefinition = _mapper.Map<WaterMeterTagDefinition>(createDto);
             await _commandService.Add(waterMeterTagDefinition);
         }
     }
     public interface IWaterMeterTagDefinitionDeleteHandler
     {
         Task Handle(WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken);
     }
     public class WaterMeterTagDefinitionDeleteHandler : IWaterMeterTagDefinitionDeleteHandler
     {
         private readonly IWaterMeterTagDefinitionCommandService _commandService;
         private readonly IWaterMeterTagDefinitionQueryService _queryService;
         public WaterMeterTagDefinitionDeleteHandler(
             IWaterMeterTagDefinitionCommandService commandService,
             IWaterMeterTagDefinitionQueryService queryService)
         {
             _commandService = commandService;
             _commandService.NotNull(nameof(commandService));

             _queryService = queryService;
             _queryService.NotNull(nameof(queryService));
         }

         public async Task Handle(WaterMeterTagDefinitionDeleteDto deleteDto, CancellationToken cancellationToken)
         {
             var waterMeterTagDefinition = await _queryService.Get(deleteDto.Id);
             if (waterMeterTagDefinition == null)
             {
                 throw new InvalidIdException();//todo : exception
             }
             await _commandService.Remove(waterMeterTagDefinition);
         }
     }


     public interface IWaterMeterTagDefinitionUpdateHandler
     {
         Task Handle(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken);
     }
     public class WaterMeterTagDefinitionUpdateHandler : IWaterMeterTagDefinitionUpdateHandler
     {
         private readonly IMapper _mapper;
         private readonly IWaterMeterTagDefinitionQueryService _queryService;
         public WaterMeterTagDefinitionUpdateHandler(
             IMapper mapper,
             IWaterMeterTagDefinitionQueryService queryService)
         {
             _mapper = mapper;
             _mapper.NotNull(nameof(mapper));

             _queryService = queryService;
             _queryService.NotNull(nameof(queryService));
         }

         public async Task Handle(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
         {
             var waterMeterTagDefinition = await _queryService.Get(updateDto.Id);
             if (waterMeterTagDefinition == null)
             {
                 throw new InvalidIdException();//todo : exception
             }
             _mapper.Map(updateDto, waterMeterTagDefinition);
         }
     }

     */
    public interface IWaterMeterTagDefinitionGetSingleHandler
    {
        Task<WaterMeterTagDefinitionGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
