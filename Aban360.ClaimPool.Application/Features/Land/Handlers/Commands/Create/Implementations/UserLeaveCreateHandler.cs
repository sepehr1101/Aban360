using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
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
