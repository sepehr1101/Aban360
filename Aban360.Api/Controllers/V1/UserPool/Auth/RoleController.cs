using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/role")]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleCreateHandler _roleCreateHandler;
        private readonly IRoleUpdateHandler _roleUpdateHandler;
        private readonly IUpdateUsersInRoleHandler _updateUsersInRoleHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRoleDeleteHandler _roleDeleteHandler;
        private readonly IRoleGetSingleHandler _roleGetSingleHandler;
        private readonly IAccessTreeValueKeyQueryHandler _accessTreeValueKeyQueryHandler;
        private readonly IRoleGetAllHandler _roleGetAllHandler;
        private readonly IRoleQueryParamsOfCreateHandler _roleQueryParamsOfCreateHandler;

        public RoleController(
            IUnitOfWork uow,
            IRoleCreateHandler roleCreateHandler,
            IRoleUpdateHandler roleUpdateHandler,
            IUpdateUsersInRoleHandler updateUsersInRoleHandler,
            IBackgroundJobClient backgroundJobClient,
            IRoleDeleteHandler roleDeleteHandler,
            IRoleGetSingleHandler roleGetSingleHandler,
            IAccessTreeValueKeyQueryHandler accessTreeValueKeyQueryHandler,
            IRoleGetAllHandler roleGetAllHandler,
            IRoleQueryParamsOfCreateHandler roleQueryParamsOfCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _roleCreateHandler = roleCreateHandler;
            _roleCreateHandler.NotNull(nameof(roleCreateHandler));

            _roleUpdateHandler = roleUpdateHandler;
            _roleUpdateHandler.NotNull(nameof(roleUpdateHandler));

            _updateUsersInRoleHandler = updateUsersInRoleHandler;
            _updateUsersInRoleHandler.NotNull(nameof(updateUsersInRoleHandler));

            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));

            _roleDeleteHandler = roleDeleteHandler;
            _roleDeleteHandler.NotNull(nameof(roleDeleteHandler));

            _roleGetSingleHandler = roleGetSingleHandler;
            _roleGetSingleHandler.NotNull(nameof(roleGetSingleHandler));

            _accessTreeValueKeyQueryHandler = accessTreeValueKeyQueryHandler;
            _accessTreeValueKeyQueryHandler.NotNull(nameof(accessTreeValueKeyQueryHandler));

            _roleGetAllHandler = roleGetAllHandler;
            _roleGetAllHandler.NotNull(nameof(roleGetAllHandler));

            _roleQueryParamsOfCreateHandler = roleQueryParamsOfCreateHandler;
            _roleQueryParamsOfCreateHandler.NotNull(nameof(_roleQueryParamsOfCreateHandler));
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _roleCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return OkEnvelope(createDto);
        }

        [Authorize]
        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _roleUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            _backgroundJobClient.Enqueue(() => _updateUsersInRoleHandler.Handle(updateDto.Id, CancellationToken.None));
            //await _updateUsersInRoleHandler.Handle(updateDto.Id, CancellationToken.None);

            return OkEnvelope(updateDto);
        }

        [Authorize]
        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] RoleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _roleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return OkEnvelope(deleteDto);
        }

        [Authorize]
        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleDisplayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            RoleDisplayDto roleDisplayDto = await _roleGetSingleHandler.Handle(id, cancellationToken);
            return OkEnvelope(roleDisplayDto);
        }

        [Authorize]
        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RoleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<RoleGetDto> role = await _roleGetAllHandler.Handle(cancellationToken);
            return OkEnvelope(role);
        }

        [HttpGet,HttpPost]
        [Route("params-create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleParamsOfCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoleParamsOfCreate(CancellationToken cancellationToken)
        {
            RoleParamsOfCreateDto roleParamsOfCreateDto= await _roleQueryParamsOfCreateHandler.Handle(cancellationToken); 
            return base.Ok(roleParamsOfCreateDto);
        }

        // Role's params-create action historically returned a raw DTO without authorization.
        // Keep authorization per action and use the shared envelope format only for the other actions.
        private IActionResult OkEnvelope<T>(T data)
        {
            var envelope = new ApiResponseEnvelope<T>(StatusCodes.Status200OK, data, MessageResources.SuccessfulProccess);
            return base.Ok(envelope);
        }

    }
}
