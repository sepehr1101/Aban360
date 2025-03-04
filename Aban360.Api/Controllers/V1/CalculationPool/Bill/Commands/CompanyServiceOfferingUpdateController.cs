using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceOfferingUpdateHandler _companyServiceOfferingUpdateHandler;
        public CompanyServiceOfferingUpdateController(
            IUnitOfWork uow,
            ICompanyServiceOfferingUpdateHandler companyServiceOfferingUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceOfferingUpdateHandler = companyServiceOfferingUpdateHandler;
            _companyServiceOfferingUpdateHandler.NotNull(nameof(companyServiceOfferingUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceOfferingUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
