using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.UserGuide.Contracts;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.UserGuide
{
    [Route("v1/faq")]
    public class FaqGetController : BaseController
    {
        private readonly IGetAllFaqsHandler _allHandler;
        private readonly IGetFaqByIdHandler _byIdHandler;
        public FaqGetController(
            IGetAllFaqsHandler allHandler,
            IGetFaqByIdHandler byIdHandler)
        {
            _allHandler = allHandler;
            _allHandler.NotNull(nameof(_allHandler));

            _byIdHandler = byIdHandler;
            _byIdHandler.NotNull(nameof(_byIdHandler));
        }

        [Route("all")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<FaqGetAllDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<FaqGetAllDto> faqs = await _allHandler.Handle(cancellationToken);
            return Ok(faqs);
        }

        [Route("{id}")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Faq>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            Faq faq = await _byIdHandler.Handle(id, cancellationToken);
            return Ok(faq);
        }
    }
}
