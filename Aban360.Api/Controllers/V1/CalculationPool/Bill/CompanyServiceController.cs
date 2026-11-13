using Aban360.CalculationPool.Application.Features.Bill.Services;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/company-service")]
    public class CompanyServiceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICompanyServiceService _service;

        public CompanyServiceController(IUnitOfWork uow, ICompanyServiceService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CompanyServiceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceGetDto companyServices = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(companyServices);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<CompanyServiceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceGetDto> companyServices = await _service.GetAllAsync(cancellationToken);
            return Ok(companyServices);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/company-service-by-typeid/all/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int id, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> companyServices = await _service.GetByTypeIdAsync(id, cancellationToken);
            return Ok(companyServices);
        }
    }
}
