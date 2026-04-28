using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment-off")]
    public class AssessmentOffController : BaseController
    {
        private readonly IAssessmentOffInsertHandler _insertHandler;
        private readonly IAssessmentOffByAssessmentCodeGetHandler _byAssessmentCodeGetHandler;
        private readonly IAssessmentOffGetAllHandler _getAllHandler;
        public AssessmentOffController(
            IAssessmentOffInsertHandler insertHandler,
            IAssessmentOffByAssessmentCodeGetHandler byAssessmentCodeGetHandler,
            IAssessmentOffGetAllHandler getAllHandler)
        {
            _insertHandler = insertHandler;
            _insertHandler.NotNull(nameof(insertHandler));

            _byAssessmentCodeGetHandler = byAssessmentCodeGetHandler;
            _byAssessmentCodeGetHandler.NotNull(nameof(byAssessmentCodeGetHandler));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(getAllHandler));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentOffInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAssessmentOff([FromBody] AssessmentOffInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _insertHandler.Handle(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("get-by-code/{assessmentCode}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentOffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByAssessmentCode(int assessmentCode, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            IEnumerable<AssessmentOffGetDto> result = await _byAssessmentCodeGetHandler.Handle(assessmentCode, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentOffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            IEnumerable<AssessmentOffGetDto> result = await _getAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
