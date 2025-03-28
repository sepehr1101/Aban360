using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/user-Workday")]
    public class UserWorkdayCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserWorkdayCreateHandler _userWorkdayCreateHandler;
        public UserWorkdayCreateController(
            IUnitOfWork uow,
            IUserWorkdayCreateHandler userWorkdayCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userWorkdayCreateHandler = userWorkdayCreateHandler;
            _userWorkdayCreateHandler.NotNull(nameof(userWorkdayCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserWorkdayCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UserWorkdayCreateDto createDto, CancellationToken cancellationToken)
        {
            await _userWorkdayCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
