using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Constants;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/user")]
    public class UserCreateController :BaseController
    {
        private IUnitOfWork _uow;
        private IUserCreateHandler _userCreateHandler;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOpLogCommandService _opLogCommandService;
        public UserCreateController(
            IUnitOfWork uow,
            IUserCreateHandler createUserHandler,
            IHttpContextAccessor contextAccessor,
            IOpLogCommandService opLogCommandService)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _userCreateHandler = createUserHandler;
            _userCreateHandler.NotNull(nameof(_userCreateHandler));

            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(_contextAccessor));

            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(_opLogCommandService));
        }
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Trigger([FromBody] UserCreateDto userCreateDto, CancellationToken cancellationToken)
        {
            await _userCreateHandler.Handle(userCreateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await InsertOpLog(userCreateDto);

            return Ok(MessageResources.CreateUserSuccess);
        }
        private async Task InsertOpLog(UserCreateDto userCreateDto)
        {
            string opLogText = string.Format(OpLogLiterals.UserCreate, userCreateDto.FullName, userCreateDto.DisplayName, userCreateDto.Mobile);
            await _opLogCommandService.Insert(opLogText, CurrentUser);
        }
    }
}