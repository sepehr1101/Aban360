using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Queries
{
    [Route("v1/user")]
    public class AssessmentGetByZoneIdController : BaseController
    {
        private readonly IAssessmentGetByZoneIdHandler _userSearch;
        public AssessmentGetByZoneIdController(IAssessmentGetByZoneIdHandler userSearch)
        {
            _userSearch = userSearch;
            _userSearch.NotNull(nameof(userSearch));
        }

        [HttpGet, HttpPost]
        [Route("assessment")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<StringDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssessmentByZoneId(SearchNumericInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<StringDictionary> assessments = await _userSearch.Handle(inputDto.Input, cancellationToken);
            return Ok(assessments);
        }
    }
}
