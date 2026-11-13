using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/company-service-offering")]
    public class CompanyServiceOfferingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceOfferingService _service;
        public CompanyServiceOfferingController(IUnitOfWork uow, ICompanyServiceOfferingService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceOfferingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceOfferingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceOfferingGetDto companyServiceOfferings = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(companyServiceOfferings);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceOfferingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceOfferingGetDto> companyServiceOfferings = await _service.GetAllAsync(cancellationToken);
            return Ok(companyServiceOfferings);
        }
    }
}
