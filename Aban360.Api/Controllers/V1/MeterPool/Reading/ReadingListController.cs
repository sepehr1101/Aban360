using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class ReadingListController : BaseController
    {
        private readonly IReaderCartableGetHandler _readerCartableGetHandler;
        public ReadingListController(IReaderCartableGetHandler readerCartableGetHandler)
        {
            _readerCartableGetHandler = readerCartableGetHandler;
            _readerCartableGetHandler.NotNull(nameof(readerCartableGetHandler));
        }

        [Route("load")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReaderCartableOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoadList(CancellationToken cancellationToken)
        {
            ReaderCartableOutputDto result = await _readerCartableGetHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
