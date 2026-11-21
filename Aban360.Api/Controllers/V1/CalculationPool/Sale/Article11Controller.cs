using Aban360.Api.Filters;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Application.Features.Sale.Services;
using Aban360.Api.Controllers.V1.CalculationPool.Sale;
using IArticle11Service = global::Aban360.CalculationPool.Application.Features.Sale.Services.IArticle11Service;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    [Route("v1/article11")]
    public class Article11Controller : BaseController
    {
        private readonly IArticle11Service _article11Service;
        public Article11Controller(IArticle11Service article11Service)
        {
            _article11Service = article11Service;
            _article11Service.NotNull(nameof(article11Service));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] Article11CreateDto createDto, CancellationToken cancellationToken)
        {
            await _article11Service.Article11CreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _article11Service.Article11DeleteAsync(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] Article11UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _article11Service.Article11UpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Article11OutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _article11Service.Article11GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11OutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            Article11OutputDto result = await _article11Service.Article11GetAsync(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
