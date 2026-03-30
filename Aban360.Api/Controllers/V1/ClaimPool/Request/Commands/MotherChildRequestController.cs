using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/request")]
    public class MotherChildRequestController : BaseController
    {
        private readonly ISetCommonSiphonRequestHandler _commonSiphonHandler;
        private readonly ISetMotherChildRequestHandler _motherChildHandler;
        private readonly IMotherUpdateHandler _motherUpdateHandler;
        private readonly IMotherDeleteHandler _motherDeleteHandler;
        public MotherChildRequestController(
            ISetCommonSiphonRequestHandler commonSiphonHandler,
            ISetMotherChildRequestHandler motherChildHandler,
            IMotherUpdateHandler motherUpdateHandler,
            IMotherDeleteHandler motherDeleteHandler)
        {
            _commonSiphonHandler = commonSiphonHandler;
            _commonSiphonHandler.NotNull(nameof(commonSiphonHandler));

            _motherChildHandler = motherChildHandler;
            _motherChildHandler.NotNull(nameof(motherChildHandler));

            _motherUpdateHandler = motherUpdateHandler;
            _motherUpdateHandler.NotNull(nameof(motherUpdateHandler));

            _motherDeleteHandler = motherDeleteHandler;
            _motherDeleteHandler.NotNull(nameof(motherDeleteHandler));
        }

        [HttpPost, HttpGet]
        [Route("set-common-siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CommonSiphonRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetCommonSiphon([FromBody] CommonSiphonRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _commonSiphonHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("update-common-siphon")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CommonSiphonUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCommonSiphon([FromBody] CommonSiphonUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _motherUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("set-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildRequestInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetMotherChild([FromBody] MotherChildRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            int userName = UserService.GetUserCode(CurrentUser.Username);
            await _motherChildHandler.Handle(inputDto, userName, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("update-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMotherChild([FromBody] MotherChildUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await _motherUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost, HttpGet]
        [Route("delete-mother-child")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MotherChildUpdateInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMotherChild([FromBody] SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            await _motherDeleteHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(inputDto);
        }
    }
}
