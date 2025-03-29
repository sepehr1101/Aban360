using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UserLeaveGetAllHandler : IUserLeaveGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveQueryService _userLeaveQueryService;
        public UserLeaveGetAllHandler(
            IMapper mapper,
            IUserLeaveQueryService userLeaveQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveQueryService = userLeaveQueryService;
            _userLeaveQueryService.NotNull(nameof(_userLeaveQueryService));
        }

        public async Task<ICollection<UserLeaveGetDto>> Handle(CancellationToken cancellationToken)
        {
            var userLeave = await _userLeaveQueryService.Get();
            return _mapper.Map<ICollection<UserLeaveGetDto>>(userLeave);
        }
    }
}
