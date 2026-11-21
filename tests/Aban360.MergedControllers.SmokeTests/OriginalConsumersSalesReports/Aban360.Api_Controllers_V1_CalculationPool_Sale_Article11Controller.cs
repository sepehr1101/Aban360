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
using Aban360.Api.Controllers.V1;
namespace BeforeSalesReportsConsumers.Aban360.Api.Controllers.V1.CalculationPool.Sale{
    [Route("v1/article11")]
    public class Article11Controller : BaseController
    {
        private readonly IArticle11CreateHadler _c0_createHadler;
        private readonly IArticle11DeleteHadler _c1_deleteHadler;
        private readonly IArticle11UpdateHadler _c2_updateHadler;
        private readonly IArticle11Service _article11Service;
        public Article11Controller(IArticle11CreateHadler c0_createHadler, IArticle11DeleteHadler c1_deleteHadler, IArticle11UpdateHadler c2_updateHadler, IArticle11Service article11Service)
        {
            _c0_createHadler = c0_createHadler;
            _c0_createHadler.NotNull(nameof(c0_createHadler));
            _c1_deleteHadler = c1_deleteHadler;
            _c1_deleteHadler.NotNull(nameof(c1_deleteHadler));
            _c2_updateHadler = c2_updateHadler;
            _c2_updateHadler.NotNull(nameof(c2_updateHadler));
            _article11Service = article11Service;
            _article11Service.NotNull(nameof(article11Service));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] Article11CreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_createHadler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _c1_deleteHadler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] Article11UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c2_updateHadler.Handle(updateDto, CurrentUser, cancellationToken);
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
