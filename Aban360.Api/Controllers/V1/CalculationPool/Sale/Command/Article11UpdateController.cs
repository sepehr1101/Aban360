using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/article11")]
    public class Article11UpdateController : BaseController
    {
        private readonly IArticle11UpdateHadler _updateHadler;
        public Article11UpdateController(IArticle11UpdateHadler updateHadler)
        {
            _updateHadler = updateHadler;
            _updateHadler.NotNull(nameof(updateHadler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] Article11UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _updateHadler.Handle(updateDto, cancellationToken);

            return Ok(updateDto);
        }
    }
}
