using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/user-Workday")]
    public class UserWorkdayController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserWorkdayCreateHandler _userWorkdayCreateHandler;
        private readonly IUserWorkdayUpdateHandler _userWorkdayUpdateHandler;
        private readonly IUserWorkdayDeleteHandler _userWorkdayDeleteHandler;
        private readonly IUserWorkdayGetSingleHandler _userWorkdayGetSingleHandler;
        private readonly IUserWorkdayGetAllHandler _userWorkdayGetAllHandler;

        public UserWorkdayController(
            IUnitOfWork uow,
            IUserWorkdayCreateHandler userWorkdayCreateHandler,
            IUserWorkdayUpdateHandler userWorkdayUpdateHandler,
            IUserWorkdayDeleteHandler userWorkdayDeleteHandler,
            IUserWorkdayGetSingleHandler userWorkdayGetSingleHandler,
            IUserWorkdayGetAllHandler userWorkdayGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userWorkdayCreateHandler = userWorkdayCreateHandler;
            _userWorkdayCreateHandler.NotNull(nameof(userWorkdayCreateHandler));

            _userWorkdayUpdateHandler = userWorkdayUpdateHandler;
            _userWorkdayUpdateHandler.NotNull(nameof(userWorkdayUpdateHandler));

            _userWorkdayDeleteHandler = userWorkdayDeleteHandler;
            _userWorkdayDeleteHandler.NotNull(nameof(userWorkdayDeleteHandler));

            _userWorkdayGetSingleHandler = userWorkdayGetSingleHandler;
            _userWorkdayGetSingleHandler.NotNull(nameof(userWorkdayGetSingleHandler));

            _userWorkdayGetAllHandler = userWorkdayGetAllHandler;
            _userWorkdayGetAllHandler.NotNull(nameof(userWorkdayGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserWorkdayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _userWorkdayUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserWorkdayDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UserWorkdayDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _userWorkdayDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserWorkdayGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var userWorkdays = await _userWorkdayGetSingleHandler.Handle(id, cancellationToken);
            return Ok(userWorkdays);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserWorkdayGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userWorkdays = await _userWorkdayGetAllHandler.Handle(cancellationToken);
            return Ok(userWorkdays);
        }
    }
}
