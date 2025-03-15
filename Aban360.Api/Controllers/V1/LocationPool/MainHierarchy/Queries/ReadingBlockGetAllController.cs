using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/reading-block")]
    public class ReadingBlocKGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockGetAllHandler _readingBlockGetAllHandler;
        public ReadingBlocKGetAllController(
            IUnitOfWork uow,
            IReadingBlockGetAllHandler readingBlockGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockGetAllHandler = readingBlockGetAllHandler;
            _readingBlockGetAllHandler.NotNull(nameof(readingBlockGetAllHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ReadingBlockGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ReadingBlockGetDto> readingBlock = await _readingBlockGetAllHandler.Handle(cancellationToken);
            return Ok(readingBlock);
        }
    }
}
