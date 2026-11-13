using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Microsoft.AspNetCore.Mvc;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    public class FaqController : BaseController
    {
        private readonly ICreateFaqHandler _c0_createHandler;
        private readonly IDeleteFaqHandler _c1_DeleteHandler;
        private readonly IGetAllFaqsHandler _c2_allHandler;
        private readonly IGetFaqByIdHandler _c2_byIdHandler;
        private readonly IUpdateFaqHandler _c3_UpdateHandler;
        public FaqController(ICreateFaqHandler c0_createHandler, IDeleteFaqHandler c1_DeleteHandler, IGetAllFaqsHandler c2_allHandler, IGetFaqByIdHandler c2_byIdHandler, IUpdateFaqHandler c3_UpdateHandler)
        {
            _c0_createHandler = c0_createHandler;
            _c0_createHandler.NotNull(nameof(_c0_createHandler));
            _c1_DeleteHandler = c1_DeleteHandler;
            _c1_DeleteHandler.NotNull(nameof(_c1_DeleteHandler));
            _c2_allHandler = c2_allHandler;
            _c2_allHandler.NotNull(nameof(_c2_allHandler));
            _c2_byIdHandler = c2_byIdHandler;
            _c2_byIdHandler.NotNull(nameof(_c2_byIdHandler));
            _c3_UpdateHandler = c3_UpdateHandler;
            _c3_UpdateHandler.NotNull(nameof(_c3_UpdateHandler));
        }

        [Route("~/v1/faq/create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _c0_createHandler.Handle(faq, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [Route("~/v1/faq/Delete/{id}")]
        [HttpPost, HttpDelete]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _c1_DeleteHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [Route("~/v1/faq/all")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<FaqGetAllDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<FaqGetAllDto> faqs = await _c2_allHandler.Handle(cancellationToken);
            return Ok(faqs);
        }

        [Route("~/v1/faq/{id}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Faq>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Faq faq = await _c2_byIdHandler.Handle(id, cancellationToken);
            return Ok(faq);
        }

        [Route("~/v1/faq/update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _c3_UpdateHandler.Handle(faq, cancellationToken);
            return Ok(id);
        }
    }
}
