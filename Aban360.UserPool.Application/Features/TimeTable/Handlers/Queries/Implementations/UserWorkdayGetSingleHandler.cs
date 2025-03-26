using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Queries.Implementations
{
    internal sealed class UserWorkdayGetSingleHandler : IUserWorkdayGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayQueryService _userWorkdayQueryService;
        public UserWorkdayGetSingleHandler(
            IMapper mapper,
            IUserWorkdayQueryService userWorkdayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayQueryService = userWorkdayQueryService;
            _userWorkdayQueryService.NotNull(nameof(_userWorkdayQueryService));
        }

        public async Task<UserWorkdayGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var userWorkday = await _userWorkdayQueryService.Get(id);
            return _mapper.Map<UserWorkdayGetDto>(userWorkday);
        }
    }
}
