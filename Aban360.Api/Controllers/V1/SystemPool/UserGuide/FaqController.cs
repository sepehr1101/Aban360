using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Microsoft.AspNetCore.Mvc;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Aban360.Api.Filters;
using Aban360.SystemPool.Application.Features.UserGuide.Services;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    [Route("v1/faq")]
    public class FaqController : BaseController
    {
        private readonly IFaqApplicationService _faqApplicationService;
        public FaqController(IFaqApplicationService faqApplicationService)
        {
            _faqApplicationService = faqApplicationService;
            _faqApplicationService.NotNull(nameof(_faqApplicationService));
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _faqApplicationService.CreateAsync(faq, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [Route("Delete/{id}")]
        [HttpPost, HttpDelete]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _faqApplicationService.DeleteAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [Route("all")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<FaqGetAllDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<FaqGetAllDto> faqs = await _faqApplicationService.GetAllAsync(cancellationToken);
            return Ok(faqs);
        }

        [Route("{id}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Faq>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Faq faq = await _faqApplicationService.GetSingleAsync(id, cancellationToken);
            return Ok(faq);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] FaqDto faq, CancellationToken cancellationToken)
        {
            faq.NotNull(nameof(faq));
            int id = await _faqApplicationService.UpdateAsync(faq, cancellationToken);
            return Ok(id);
        }
    }
}
