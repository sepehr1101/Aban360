using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Db70
{
    [Route("v1/bill-return-cause")]
    public class BillReturnCauseController : BaseController
    {
        private readonly IBillReturnCauseCreateHandler _billReturnCauseCreateHandler;
        private readonly IBillReturnCauseDeleteHandler _billReturnCauseDeleteHandler;
        private readonly IBillReturnCauseUpdateHandler _billReturnCauseUpdateHandler;
        private readonly IBillReturnCauseGetAllHandler _billReturnCauseGetAllHandler;
        private readonly IBillReturnCauseGetHandler _billReturnCauseGetHandler;

        public BillReturnCauseController(
            IBillReturnCauseCreateHandler billReturnCauseCreateHandler,
            IBillReturnCauseDeleteHandler billReturnCauseDeleteHandler,
            IBillReturnCauseUpdateHandler billReturnCauseUpdateHandler,
            IBillReturnCauseGetAllHandler billReturnCauseGetAllHandler,
            IBillReturnCauseGetHandler billReturnCauseGetHandler)
        {
            _billReturnCauseCreateHandler = billReturnCauseCreateHandler;
            _billReturnCauseCreateHandler.NotNull(nameof(billReturnCauseCreateHandler));
            _billReturnCauseDeleteHandler = billReturnCauseDeleteHandler;
            _billReturnCauseDeleteHandler.NotNull(nameof(billReturnCauseDeleteHandler));
            _billReturnCauseUpdateHandler = billReturnCauseUpdateHandler;
            _billReturnCauseUpdateHandler.NotNull(nameof(billReturnCauseUpdateHandler));
            _billReturnCauseGetAllHandler = billReturnCauseGetAllHandler;
            _billReturnCauseGetAllHandler.NotNull(nameof(billReturnCauseGetAllHandler));
            _billReturnCauseGetHandler = billReturnCauseGetHandler;
            _billReturnCauseGetHandler.NotNull(nameof(billReturnCauseGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(BillReturnCauseCreateDto createDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseCreateHandler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(BillReturnCauseDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseDeleteHandler.Handle(deleteDto, CurrentUser, cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(BillReturnCauseUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _billReturnCauseUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> result = await _billReturnCauseGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillReturnCauseGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(short id, CancellationToken cancellationToken)
        {
            SearchShortInputDto inputDto = id;
            BillReturnCauseGetDto result = await _billReturnCauseGetHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
