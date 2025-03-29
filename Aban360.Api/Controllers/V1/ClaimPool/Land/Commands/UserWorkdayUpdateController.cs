using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/user-Workday")]
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
