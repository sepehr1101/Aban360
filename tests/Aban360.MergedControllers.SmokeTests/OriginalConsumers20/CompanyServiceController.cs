// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.CalculationPool.Bill;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/company-service")]
    public class CompanyServiceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceCreateHandler _companyServiceCreateHandler;
        private readonly ICompanyServiceUpdateHandler _companyServiceUpdateHandler;
        private readonly ICompanyServiceDeleteHandler _companyServiceDeleteHandler;
        private readonly ICompanyServiceGetSingleHandler _companyServiceGetSingleHandler;
        private readonly ICompanyServiceGetAllHandler _companyServiceGetAllHandler;
        private readonly ICompanyServiceGetByTypeIdHandler _companyServiceGetByTypeIdHandler;

        public CompanyServiceController(
            IUnitOfWork uow,
            ICompanyServiceCreateHandler companyServiceCreateHandler,
            ICompanyServiceUpdateHandler companyServiceUpdateHandler,
            ICompanyServiceDeleteHandler companyServiceDeleteHandler,
            ICompanyServiceGetSingleHandler companyServiceGetSingleHandler,
            ICompanyServiceGetAllHandler companyServiceGetAllHandler,
            ICompanyServiceGetByTypeIdHandler companyServiceGetByTypeIdHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _companyServiceCreateHandler = companyServiceCreateHandler;
            _companyServiceCreateHandler.NotNull(nameof(companyServiceCreateHandler));

            _companyServiceUpdateHandler = companyServiceUpdateHandler;
            _companyServiceUpdateHandler.NotNull(nameof(companyServiceUpdateHandler));

            _companyServiceDeleteHandler = companyServiceDeleteHandler;
            _companyServiceDeleteHandler.NotNull(nameof(companyServiceDeleteHandler));

            _companyServiceGetSingleHandler = companyServiceGetSingleHandler;
            _companyServiceGetSingleHandler.NotNull(nameof(companyServiceGetSingleHandler));

            _companyServiceGetAllHandler = companyServiceGetAllHandler;
            _companyServiceGetAllHandler.NotNull(nameof(companyServiceGetAllHandler));

            _companyServiceGetByTypeIdHandler = companyServiceGetByTypeIdHandler;
            _companyServiceGetByTypeIdHandler.NotNull(nameof(companyServiceGetByTypeIdHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _companyServiceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _companyServiceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _companyServiceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceGetDto companyServices = await _companyServiceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(companyServices);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceGetDto> companyServices = await _companyServiceGetAllHandler.Handle(cancellationToken);
            return Ok(companyServices);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/company-service-by-typeid/all/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> companyServices = await _companyServiceGetByTypeIdHandler.Handle(id, cancellationToken);
            return Ok(companyServices);
        }
    }
}
