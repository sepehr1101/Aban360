using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/user-leave")]
    public class UserLeaveCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserLeaveCreateHandler _userLeaveCreateHandler;
        public UserLeaveCreateController(
            IUnitOfWork uow,
            IUserLeaveCreateHandler userLeaveCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLeaveCreateHandler = userLeaveCreateHandler;
            _userLeaveCreateHandler.NotNull(nameof(userLeaveCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UserLeaveCreateDto createDto, CancellationToken cancellationToken)
        {
            await _userLeaveCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
