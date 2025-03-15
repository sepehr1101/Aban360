using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingGetAllController : BaseController
    {
        private readonly ICompanyServiceOfferingGetAllHandler _companyServiceOfferingGetAllHandler;
        public CompanyServiceOfferingGetAllController(ICompanyServiceOfferingGetAllHandler companyServiceOfferingGetAllHandler)
        {
            _companyServiceOfferingGetAllHandler = companyServiceOfferingGetAllHandler;
            _companyServiceOfferingGetAllHandler.NotNull(nameof(companyServiceOfferingGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceOfferingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceOfferingGetDto> companyServiceOfferings = await _companyServiceOfferingGetAllHandler.Handle(cancellationToken);
            return Ok(companyServiceOfferings);
        }
    }
}
