using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    public class HeadquarterCreateHandler : IHeadquarterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHeadquarterCommandService _headquarterCommandService;
        public HeadquarterCreateHandler(
            IMapper mapper,
            IHeadquarterCommandService headquarterCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _headquarterCommandService = headquarterCommandService;
            _headquarterCommandService.NotNull(nameof(headquarterCommandService));
        }

        public async Task Handle(HeadquarterCreateDto createDto, CancellationToken cancellationToken)
        {
            var headquarter = _mapper.Map<Headquarters>(createDto);
            await _headquarterCommandService.Add(headquarter);
        }
    }
}
