using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UserWorkdayCreateHandler : IUserWorkdayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayCommandService _userWorkdayCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddhoc;
        public UserWorkdayCreateHandler(
            IMapper mapper,
            IUserWorkdayCommandService userWorkdayCommandService,
            IZoneTitleAddhoc zoneTitleAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayCommandService = userWorkdayCommandService;
            _userWorkdayCommandService.NotNull(nameof(_userWorkdayCommandService));

            _zoneTitleAddhoc=zoneTitleAddhoc;
            _zoneTitleAddhoc.NotNull(nameof(_zoneTitleAddhoc));
        }

        public async Task Handle(UserWorkdayCreateDto createDto, CancellationToken cancellationToken)
        {
            var userWorkday = _mapper.Map<UserWorkday>(createDto);
            var zoneTitle=await _zoneTitleAddhoc.Handle(createDto.ZoneId,cancellationToken);

            userWorkday.ZoneTitle = zoneTitle;
            await _userWorkdayCommandService.Add(userWorkday);
        }
    }
}
