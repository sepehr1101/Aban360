using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Features.TimeTable.Queries.Contracts;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Implementations
{
    internal sealed class UserWorkdayUpdateHandler : IUserWorkdayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserWorkdayQueryService _userWorkdayQueryService;
        public UserWorkdayUpdateHandler(
            IMapper mapper,
            IUserWorkdayQueryService userWorkdayQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userWorkdayQueryService = userWorkdayQueryService;
            _userWorkdayQueryService.NotNull(nameof(_userWorkdayQueryService));
        }

        public async Task Handle(UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var userWorkday = await _userWorkdayQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, userWorkday);
        }
    }
}
