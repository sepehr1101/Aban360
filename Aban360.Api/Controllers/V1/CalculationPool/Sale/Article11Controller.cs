using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    public class Article11Controller : BaseController
    {
        private readonly IArticle11CreateHadler _c0_createHadler;
        private readonly IArticle11DeleteHadler _c1_deleteHadler;
        private readonly IArticle11UpdateHadler _c2_updateHadler;
        private readonly IArticle11GetAllHandler _c3_getHandler;
        private readonly IArticle11GetHandler _c4_getHandler;
        public Article11Controller(IArticle11CreateHadler c0_createHadler, IArticle11DeleteHadler c1_deleteHadler, IArticle11UpdateHadler c2_updateHadler, IArticle11GetAllHandler c3_getHandler, IArticle11GetHandler c4_getHandler)
        {
            _c0_createHadler = c0_createHadler;
            _c0_createHadler.NotNull(nameof(c0_createHadler));
            _c1_deleteHadler = c1_deleteHadler;
            _c1_deleteHadler.NotNull(nameof(c1_deleteHadler));
            _c2_updateHadler = c2_updateHadler;
            _c2_updateHadler.NotNull(nameof(c2_updateHadler));
            _c3_getHandler = c3_getHandler;
            _c3_getHandler.NotNull(nameof(c3_getHandler));
            _c4_getHandler = c4_getHandler;
            _c4_getHandler.NotNull(nameof(c4_getHandler));
        }

        [HttpPost]
        [Route("~/v1/article11/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] Article11CreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_createHadler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("~/v1/article11/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _c1_deleteHadler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("~/v1/article11/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] Article11UpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c2_updateHadler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("~/v1/article11/get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Article11OutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Article11OutputDto> result = await _c3_getHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/article11/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Article11OutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            Article11OutputDto result = await _c4_getHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
