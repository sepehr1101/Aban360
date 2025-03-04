using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceTypeUpdateHandler _companyServiceTypeUpdateHandler;
        public CompanyServiceTypeUpdateController(
            IUnitOfWork uow,
            ICompanyServiceTypeUpdateHandler companyServiceTypeUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceTypeUpdateHandler = companyServiceTypeUpdateHandler;
            _companyServiceTypeUpdateHandler.NotNull(nameof(companyServiceTypeUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
