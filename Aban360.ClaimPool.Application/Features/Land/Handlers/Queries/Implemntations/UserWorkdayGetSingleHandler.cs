using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
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
