using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceTypeDeleteHandler _companyServiceTypeDeleteHandler;
        public CompanyServiceTypeDeleteController(
            IUnitOfWork uow,
            ICompanyServiceTypeDeleteHandler companyServiceTypeDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceTypeDeleteHandler = companyServiceTypeDeleteHandler;
            _companyServiceTypeDeleteHandler.NotNull(nameof(companyServiceTypeDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _companyServiceTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
