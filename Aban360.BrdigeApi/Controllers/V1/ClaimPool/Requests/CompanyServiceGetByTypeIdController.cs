using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ClaimPool.Requests
{
    [Route("v1/company-service-by-typeid")]
    public class CompanyServiceGetByTypeIdController : BaseController
    {
        private readonly ICompanyServiceService _service;

        public CompanyServiceGetByTypeIdController(ICompanyServiceService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpGet]
        [Route("all/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> companyServices = await _service.GetByTypeIdAsync(id, cancellationToken);
            return Ok(companyServices);
        }
    }
}
