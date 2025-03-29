using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/user-Workday")]
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
