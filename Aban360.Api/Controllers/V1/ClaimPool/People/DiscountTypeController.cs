using Aban360.ClaimPool.Application.Features.People.Services;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1.ClaimPool.People
{
    [Route("v1/discount-type")]
    public class DiscountTypeController : BaseController
    {
        private readonly IDiscountTypeService _service;

        public DiscountTypeController(IDiscountTypeService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([Optional] bool isReturn, CancellationToken cancellationToken)
        {
            var discountTypes = await _service.GetAllAsync(isReturn, cancellationToken);
            return Ok(discountTypes);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DiscountTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(DiscountTypeEnum id, CancellationToken cancellationToken)
        {
            var discountTypes = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(discountTypes);
        }
    }
}
