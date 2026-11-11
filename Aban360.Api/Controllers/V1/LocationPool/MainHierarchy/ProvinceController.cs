using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/province")]
    public class ProvinceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProvinceCreateHandler _provinceCreateHandler;
        private readonly IProvinceUpdateHandler _provinceUpdateHandler;
        private readonly IProvinceDeleteHandler _provinceDeleteHandler;
        private readonly IProvinceGetSingleHandler _provinceGetSingleHandler;
        private readonly IProvinceGetAllHandler _provinceGetAllHandler;

        public ProvinceController(
            IUnitOfWork uow,
            IProvinceCreateHandler provinceCreateHandler,
            IProvinceUpdateHandler provinceUpdateHandler,
            IProvinceDeleteHandler provinceDeleteHandler,
            IProvinceGetSingleHandler provinceGetSingleHandler,
            IProvinceGetAllHandler provinceGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _provinceCreateHandler = provinceCreateHandler;
            _provinceCreateHandler.NotNull(nameof(provinceCreateHandler));

            _provinceUpdateHandler = provinceUpdateHandler;
            _provinceUpdateHandler.NotNull(nameof(provinceUpdateHandler));

            _provinceDeleteHandler = provinceDeleteHandler;
            _provinceDeleteHandler.NotNull(nameof(provinceDeleteHandler));

            _provinceGetSingleHandler = provinceGetSingleHandler;
            _provinceGetSingleHandler.NotNull(nameof(provinceGetSingleHandler));

            _provinceGetAllHandler = provinceGetAllHandler;
            _provinceGetAllHandler.NotNull(nameof(provinceGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProvinceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _provinceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ProvinceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _provinceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ProvinceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _provinceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProvinceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            ProvinceGetDto result =await _provinceGetSingleHandler.Handle(id,cancellationToken); 
            return Ok(result);
        }

        [HttpGet,HttpPost]
        [Route("All")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ProvinceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ProvinceGetDto> result = await _provinceGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
