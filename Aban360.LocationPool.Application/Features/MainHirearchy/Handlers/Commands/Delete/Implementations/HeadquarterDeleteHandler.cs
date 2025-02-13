using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Delete.Implementations
{
    public class HeadquarterDeleteHandler : IHeadquarterDeleteHandler
    {
        private readonly IHeadquarterCommandService _headquarterCommandService;
        private readonly IHeadquarterQueryService _headquarterQueryService;
        public HeadquarterDeleteHandler(
            IHeadquarterCommandService headquarterCommandService,
            IHeadquarterQueryService headquarterQueryService)
        {
            _headquarterCommandService = headquarterCommandService;
            _headquarterCommandService.NotNull(nameof(headquarterCommandService));

            _headquarterQueryService = headquarterQueryService;
            _headquarterQueryService.NotNull(nameof(headquarterQueryService));
        }

        public async Task Handle(HeadquarterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var headquarter = await _headquarterQueryService.Get(deleteDto.Id);
            if (headquarter == null)
            {
                throw new InvalidDataException();
            }
            await _headquarterCommandService.Remove(headquarter);
        }
    }
}
