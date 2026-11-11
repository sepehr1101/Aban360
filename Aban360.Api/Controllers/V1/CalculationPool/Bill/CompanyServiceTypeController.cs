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

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/company-service-type")]
    public class CompanyServiceTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceTypeCreateHandler _companyServiceTypeCreateHandler;
        private readonly ICompanyServiceTypeUpdateHandler _companyServiceTypeUpdateHandler;
        private readonly ICompanyServiceTypeDeleteHandler _companyServiceTypeDeleteHandler;
        private readonly ICompanyServiceTypeGetSingleHandler _companyServiceTypeGetSingleHandler;
        private readonly ICompanyServiceTypeGetAllHandler _companyServiceTypeGetAllHandler;

        public CompanyServiceTypeController(
            IUnitOfWork uow,
            ICompanyServiceTypeCreateHandler companyServiceTypeCreateHandler,
            ICompanyServiceTypeUpdateHandler companyServiceTypeUpdateHandler,
            ICompanyServiceTypeDeleteHandler companyServiceTypeDeleteHandler,
            ICompanyServiceTypeGetSingleHandler companyServiceTypeGetSingleHandler,
            ICompanyServiceTypeGetAllHandler companyServiceTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceTypeCreateHandler = companyServiceTypeCreateHandler;
            _companyServiceTypeCreateHandler.NotNull(nameof(companyServiceTypeCreateHandler));

            _companyServiceTypeUpdateHandler = companyServiceTypeUpdateHandler;
            _companyServiceTypeUpdateHandler.NotNull(nameof(companyServiceTypeUpdateHandler));

            _companyServiceTypeDeleteHandler = companyServiceTypeDeleteHandler;
            _companyServiceTypeDeleteHandler.NotNull(nameof(companyServiceTypeDeleteHandler));

            _companyServiceTypeGetSingleHandler = companyServiceTypeGetSingleHandler;
            _companyServiceTypeGetSingleHandler.NotNull(nameof(companyServiceTypeGetSingleHandler));

            _companyServiceTypeGetAllHandler = companyServiceTypeGetAllHandler;
            _companyServiceTypeGetAllHandler.NotNull(nameof(companyServiceTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceTypeGetDto companyServiceTypes = await _companyServiceTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServiceTypes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceTypeGetDto> companyServiceTypes = await _companyServiceTypeGetAllHandler.Handle(cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
