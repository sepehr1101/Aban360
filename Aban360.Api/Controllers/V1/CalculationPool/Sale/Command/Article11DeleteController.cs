using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/article11")]
    public class Article11DeleteController : BaseController
    {
        private readonly IArticle11DeleteHadler _deleteHadler;
        public Article11DeleteController(IArticle11DeleteHadler deleteHadler)
        {
            _deleteHadler = deleteHadler;
            _deleteHadler.NotNull(nameof(deleteHadler));
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _deleteHadler.Handle(input, CurrentUser, cancellationToken);

            return Ok(input);
        }
    }
}
