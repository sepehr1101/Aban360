using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using Aban360.UserPool.Persistence.Features.TimeTable.Commands.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Implementations
{
    internal sealed class UserLeaveCreateHandler : IUserLeaveCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveCommandService _userLeaveCommandService;
        public UserLeaveCreateHandler(
            IMapper mapper,
            IUserLeaveCommandService userLeaveCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveCommandService = userLeaveCommandService;
            _userLeaveCommandService.NotNull(nameof(_userLeaveCommandService));
        }

        public async Task Handle(UserLeaveCreateDto createDto, CancellationToken cancellationToken)
        {
            var userLeave = _mapper.Map<UserLeave>(createDto);
            await _userLeaveCommandService.Add(userLeave);
        }
    }
}
