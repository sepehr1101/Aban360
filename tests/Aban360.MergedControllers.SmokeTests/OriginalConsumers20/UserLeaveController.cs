// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Land;
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

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/user-leave")]
    public class UserLeaveController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserLeaveCreateHandler _userLeaveCreateHandler;
        private readonly IUserLeaveUpdateHandler _userLeaveUpdateHandler;
        private readonly IUserLeaveDeleteHandler _userLeaveDeleteHandler;
        private readonly IUserLeaveGetSingleHandler _userLeaveGetSingleHandler;
        private readonly IUserLeaveGetAllHandler _userLeaveGetAllHandler;

        public UserLeaveController(
            IUnitOfWork uow,
            IUserLeaveCreateHandler userLeaveCreateHandler,
            IUserLeaveUpdateHandler userLeaveUpdateHandler,
            IUserLeaveDeleteHandler userLeaveDeleteHandler,
            IUserLeaveGetSingleHandler userLeaveGetSingleHandler,
            IUserLeaveGetAllHandler userLeaveGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _userLeaveCreateHandler = userLeaveCreateHandler;
            _userLeaveCreateHandler.NotNull(nameof(userLeaveCreateHandler));

            _userLeaveUpdateHandler = userLeaveUpdateHandler;
            _userLeaveUpdateHandler.NotNull(nameof(userLeaveUpdateHandler));

            _userLeaveDeleteHandler = userLeaveDeleteHandler;
            _userLeaveDeleteHandler.NotNull(nameof(userLeaveDeleteHandler));

            _userLeaveGetSingleHandler = userLeaveGetSingleHandler;
            _userLeaveGetSingleHandler.NotNull(nameof(userLeaveGetSingleHandler));

            _userLeaveGetAllHandler = userLeaveGetAllHandler;
            _userLeaveGetAllHandler.NotNull(nameof(userLeaveGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UserLeaveUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _userLeaveUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UserLeaveDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _userLeaveDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UserLeaveGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var userLeaves = await _userLeaveGetSingleHandler.Handle(id, cancellationToken);
            return Ok(userLeaves);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UserLeaveGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var userLeaves = await _userLeaveGetAllHandler.Handle(cancellationToken);
            return Ok(userLeaves);
        }
    }
}
