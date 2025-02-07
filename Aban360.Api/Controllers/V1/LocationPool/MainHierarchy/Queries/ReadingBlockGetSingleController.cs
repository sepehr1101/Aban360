using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Queries
{
    [Route("v1/reading-block")]
    public class ReadingBlocKGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IReadingBlockGetSingleHandler _readingBlockGetSingleHandler;
        public ReadingBlocKGetSingleController(
            IUnitOfWork uow,
            IReadingBlockGetSingleHandler readingBlockGetSingleHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _readingBlockGetSingleHandler = readingBlockGetSingleHandler;
            _readingBlockGetSingleHandler.NotNull(nameof(readingBlockGetSingleHandler));
        }

        [HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var readingBlock = await _readingBlockGetSingleHandler.Handle(id, cancellationToken);
            return Ok(readingBlock);
        }
    }
}
