using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class UserLeaveGetSingleHandler : IUserLeaveGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveQueryService _userLeaveQueryService;
        public UserLeaveGetSingleHandler(
            IMapper mapper,
            IUserLeaveQueryService userLeaveQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveQueryService = userLeaveQueryService;
            _userLeaveQueryService.NotNull(nameof(_userLeaveQueryService));
        }

        public async Task<UserLeaveGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var userLeave = await _userLeaveQueryService.Get(id);
            return _mapper.Map<UserLeaveGetDto>(userLeave);
        }
    }
}
