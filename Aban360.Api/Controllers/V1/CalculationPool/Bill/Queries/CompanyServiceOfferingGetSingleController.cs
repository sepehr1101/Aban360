using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingGetSingleController : BaseController
    {
        private readonly ICompanyServiceOfferingGetSingleHandler _companyServiceOfferingGetSingleHandler;
        public CompanyServiceOfferingGetSingleController(ICompanyServiceOfferingGetSingleHandler companyServiceOfferingGetSingleHandler)
        {
            _companyServiceOfferingGetSingleHandler = companyServiceOfferingGetSingleHandler;
            _companyServiceOfferingGetSingleHandler.NotNull(nameof(companyServiceOfferingGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var companyServiceOfferings = await _companyServiceOfferingGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServiceOfferings);
        }
    }
}
