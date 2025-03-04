using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeGetAllController : BaseController
    {
        private readonly ICompanyServiceTypeGetAllHandler _companyServiceTypeGetAllHandler;
        public CompanyServiceTypeGetAllController(ICompanyServiceTypeGetAllHandler companyServiceTypeGetAllHandler)
        {
            _companyServiceTypeGetAllHandler = companyServiceTypeGetAllHandler;
            _companyServiceTypeGetAllHandler.NotNull(nameof(companyServiceTypeGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var companyServiceTypes = await _companyServiceTypeGetAllHandler.Handle(cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
