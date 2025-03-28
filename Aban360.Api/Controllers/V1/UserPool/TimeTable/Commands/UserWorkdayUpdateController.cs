using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.TimeTable.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.TimeTable.Commands
{
    [Route("v4/user-Workday")]
    public class UserWorkdayUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserWorkdayUpdateHandler _userWorkdayUpdateHandler;
        public UserWorkdayUpdateController(
            IUnitOfWork uow,
            IUserWorkdayUpdateHandler userWorkdayUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userWorkdayUpdateHandler = userWorkdayUpdateHandler;
            _userWorkdayUpdateHandler.NotNull(nameof(userWorkdayUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _userWorkdayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
