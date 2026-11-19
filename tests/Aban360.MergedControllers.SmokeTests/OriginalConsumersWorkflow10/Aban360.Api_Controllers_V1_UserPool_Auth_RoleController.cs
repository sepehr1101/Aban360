using global::Aban360.Api.Controllers.V1.UserPool.Auth;
using Microsoft.Extensions.Logging;
using global::Aban360.Api.Controllers.V1;
using global::Aban360.Common.Categories.ApiResponse;
using global::Aban360.Common.Extensions;
using global::Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using global::Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using global::Aban360.UserPool.Domain.Constants;
using global::Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using global::Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using global::Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using global::Aban360.UserPool.Application.Features.Auth.Services;

namespace BeforeConsumersWorkflow10.Aban360.Api.Controllers.V1.UserPool.Auth{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v1/role")]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleService _roleService;
        private readonly IUpdateUsersInRoleHandler _updateUsersInRoleHandler;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IAccessTreeValueKeyQueryHandler _accessTreeValueKeyQueryHandler;
        public RoleController(IUnitOfWork uow, IRoleService roleService, IUpdateUsersInRoleHandler updateUsersInRoleHandler, IBackgroundJobClient backgroundJobClient, IAccessTreeValueKeyQueryHandler accessTreeValueKeyQueryHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _roleService = roleService;
            _roleService.NotNull(nameof(roleService));
            _updateUsersInRoleHandler = updateUsersInRoleHandler;
            _updateUsersInRoleHandler.NotNull(nameof(updateUsersInRoleHandler));
            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));
            _accessTreeValueKeyQueryHandler = accessTreeValueKeyQueryHandler;
            _accessTreeValueKeyQueryHandler.NotNull(nameof(accessTreeValueKeyQueryHandler));
            _roleService.NotNull(nameof(_roleService));
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _roleService.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return OkEnvelope(createDto);
        }

        [Authorize]
        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _roleService.UpdateAsync(updateDto, cancellationToken);
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
            await _roleService.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return OkEnvelope(deleteDto);
        }

        [Authorize]
        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleDisplayDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            RoleDisplayDto roleDisplayDto = await _roleService.GetSingleAsync(id, cancellationToken);
            return OkEnvelope(roleDisplayDto);
        }

        [Authorize]
        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<RoleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<RoleGetDto> role = await _roleService.GetAllAsync(cancellationToken);
            return OkEnvelope(role);
        }

        [HttpGet, HttpPost]
        [Route("params-create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<RoleParamsOfCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoleParamsOfCreate(CancellationToken cancellationToken)
        {
            RoleParamsOfCreateDto roleParamsOfCreateDto = await _roleService.GetCreateParamsAsync(cancellationToken);
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
