using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Implementations
{
    internal sealed class UserLeaveUpdateHandler : IUserLeaveUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveQueryService _userLeaveQueryService;
        public UserLeaveUpdateHandler(
            IMapper mapper,
            IUserLeaveQueryService userLeaveQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveQueryService = userLeaveQueryService;
            _userLeaveQueryService.NotNull(nameof(_userLeaveQueryService));
        }

        public async Task Handle(UserLeaveUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var userLeave = await _userLeaveQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, userLeave);
        }
    }
}
