using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/article11")]
    public class Article11CreateController : BaseController
    {
        private readonly IArticle11CreateHadler _createHadler;
        public Article11CreateController(IArticle11CreateHadler createHadler)
        {
            _createHadler = createHadler;
            _createHadler.NotNull(nameof(createHadler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11InputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] Article11InputDto createDto, CancellationToken cancellationToken)
        {
            await _createHadler.Handle(createDto, cancellationToken);

            return Ok(createDto);
        }
        [HttpPost]
        [Route("create/ALL")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11InputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Createall([FromBody] IEnumerable<Article11InputDto> createDto, CancellationToken cancellationToken)
        {
            foreach (var item in createDto)
            {
                await _createHadler.Handle(item, cancellationToken);

            }

            return Ok(createDto);
        }
    }
}
