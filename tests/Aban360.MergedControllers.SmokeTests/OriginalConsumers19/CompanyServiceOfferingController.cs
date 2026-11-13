using Aban360.Api.Controllers.V1;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceOfferingCreateHandler _companyServiceOfferingCreateHandler;
        private readonly ICompanyServiceOfferingUpdateHandler _companyServiceOfferingUpdateHandler;
        private readonly ICompanyServiceOfferingDeleteHandler _companyServiceOfferingDeleteHandler;
        private readonly ICompanyServiceOfferingGetSingleHandler _companyServiceOfferingGetSingleHandler;
        private readonly ICompanyServiceOfferingGetAllHandler _companyServiceOfferingGetAllHandler;

        public CompanyServiceOfferingController(
            IUnitOfWork uow,
            ICompanyServiceOfferingCreateHandler companyServiceOfferingCreateHandler,
            ICompanyServiceOfferingUpdateHandler companyServiceOfferingUpdateHandler,
            ICompanyServiceOfferingDeleteHandler companyServiceOfferingDeleteHandler,
            ICompanyServiceOfferingGetSingleHandler companyServiceOfferingGetSingleHandler,
            ICompanyServiceOfferingGetAllHandler companyServiceOfferingGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceOfferingCreateHandler = companyServiceOfferingCreateHandler;
            _companyServiceOfferingCreateHandler.NotNull(nameof(companyServiceOfferingCreateHandler));

            _companyServiceOfferingUpdateHandler = companyServiceOfferingUpdateHandler;
            _companyServiceOfferingUpdateHandler.NotNull(nameof(companyServiceOfferingUpdateHandler));

            _companyServiceOfferingDeleteHandler = companyServiceOfferingDeleteHandler;
            _companyServiceOfferingDeleteHandler.NotNull(nameof(companyServiceOfferingDeleteHandler));

            _companyServiceOfferingGetSingleHandler = companyServiceOfferingGetSingleHandler;
            _companyServiceOfferingGetSingleHandler.NotNull(nameof(companyServiceOfferingGetSingleHandler));

            _companyServiceOfferingGetAllHandler = companyServiceOfferingGetAllHandler;
            _companyServiceOfferingGetAllHandler.NotNull(nameof(companyServiceOfferingGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceOfferingCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceOfferingUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceOfferingGetDto companyServiceOfferings = await _companyServiceOfferingGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServiceOfferings);
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
