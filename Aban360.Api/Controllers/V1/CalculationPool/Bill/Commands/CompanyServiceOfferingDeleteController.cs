using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceOfferingDeleteHandler _companyServiceOfferingDeleteHandler;
        public CompanyServiceOfferingDeleteController(
            IUnitOfWork uow,
            ICompanyServiceOfferingDeleteHandler companyServiceOfferingDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceOfferingDeleteHandler = companyServiceOfferingDeleteHandler;
            _companyServiceOfferingDeleteHandler.NotNull(nameof(companyServiceOfferingDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceOfferingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _companyServiceOfferingDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
