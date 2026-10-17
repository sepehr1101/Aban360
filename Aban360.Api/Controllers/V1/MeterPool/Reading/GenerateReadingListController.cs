using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class GenerateReadingListController : BaseController
    {
        private readonly IGenerateReadingListCreateHandler _generateReadingListCreateHandler;
        public GenerateReadingListController(IGenerateReadingListCreateHandler generateReadingListCreateHandler)
        {
            _generateReadingListCreateHandler = generateReadingListCreateHandler;
            _generateReadingListCreateHandler.NotNull(nameof(generateReadingListCreateHandler));
        }

        [Route("generate-list")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GenerateReadingListOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateList([FromBody] GenerateReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            GenerateReadingListOutputDto result = await _generateReadingListCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
