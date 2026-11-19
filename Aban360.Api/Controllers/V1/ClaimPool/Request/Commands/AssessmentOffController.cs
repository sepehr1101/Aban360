using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Commands
{
    [Route("v1/assessment-off")]
    public class AssessmentOffController : BaseController
    {
        private readonly IAssessmentOffService _assessmentOffService;
        public AssessmentOffController(IAssessmentOffService assessmentOffService)
        {
            _assessmentOffService = assessmentOffService;
            _assessmentOffService.NotNull(nameof(assessmentOffService));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentOffInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAssessmentOff([FromBody] AssessmentOffInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _assessmentOffService.CreateAsync(inputDto, examinerCode, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAssessmentOff(GuidInput inputDto, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            await _assessmentOffService.DeleteAsync(inputDto.Input, examinerCode, cancellationToken);
            return Ok(inputDto.Input);
        }

        [HttpPost]
        [Route("get-by-code/{assessmentCode}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentOffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByAssessmentCode(int assessmentCode, CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            IEnumerable<AssessmentOffGetDto> result = await _assessmentOffService.GetByAssessmentCodeAsync(assessmentCode, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AssessmentOffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            int examinerCode = UserService.GetUserCode(CurrentUser.Username);
            IEnumerable<AssessmentOffGetDto> result = await _assessmentOffService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
    }
}
