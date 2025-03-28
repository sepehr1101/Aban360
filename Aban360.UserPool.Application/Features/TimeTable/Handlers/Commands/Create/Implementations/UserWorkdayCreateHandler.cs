using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Implementations
{
    internal sealed class UserWorkdayCreateHandler : IUserWorkdayCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayCommandService _userWorkdayCommandService;
        public UserWorkdayCreateHandler(
            IMapper mapper,
            IUserWorkdayCommandService userWorkdayCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayCommandService = userWorkdayCommandService;
            _userWorkdayCommandService.NotNull(nameof(_userWorkdayCommandService));
        }

        public async Task Handle(UserWorkdayCreateDto createDto, CancellationToken cancellationToken)
        {
            var userWorkday = _mapper.Map<UserWorkday>(createDto);
            await _userWorkdayCommandService.Add(userWorkday);
        }
    }
}
