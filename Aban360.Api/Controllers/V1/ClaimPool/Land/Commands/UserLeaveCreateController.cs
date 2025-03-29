using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/user-leave")]
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
