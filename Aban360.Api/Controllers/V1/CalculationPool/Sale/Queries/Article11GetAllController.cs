using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/article11")]
    public class Article11GetAllController : BaseController
    {
        private readonly IArticle11GetAllHadler _getHandler;
        public Article11GetAllController(IArticle11GetAllHadler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Article11OutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _getHandler.Handle(cancellationToken);

            return Ok(result);
        }
    }
}
