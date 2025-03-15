using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class HeadquarterUpdateHandler : IHeadquarterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHeadquarterQueryService _headquarterQueryService;
        public HeadquarterUpdateHandler(
            IMapper mapper,
            IHeadquarterQueryService headquarterQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _headquarterQueryService = headquarterQueryService;
            _headquarterQueryService.NotNull(nameof(headquarterQueryService));

        }

        public async Task Handle(HeadquarterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Headquarters headquarter = await _headquarterQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, headquarter);
        }
    }
}
