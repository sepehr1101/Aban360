using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/company-service")]
    public class CompanyServiceGetAllController : BaseController
    {
        private readonly ICompanyServiceGetAllHandler _companyServiceGetAllHandler;
        public CompanyServiceGetAllController(ICompanyServiceGetAllHandler companyServiceGetAllHandler)
        {
            _companyServiceGetAllHandler = companyServiceGetAllHandler;
            _companyServiceGetAllHandler.NotNull(nameof(companyServiceGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var companyServices = await _companyServiceGetAllHandler.Handle(cancellationToken);
            return Ok(companyServices);
        }
    }
}
