using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Queries
{
    [Route("v1/article11")]
    public class Article11GetController : BaseController
    {
        private readonly IArticle11GetHadler _getHandler;
        public Article11GetController(IArticle11GetHadler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11OutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            Article11OutputDto result = await _getHandler.Handle(inputDto, cancellationToken);

            return Ok(result);
        }
    }
}
