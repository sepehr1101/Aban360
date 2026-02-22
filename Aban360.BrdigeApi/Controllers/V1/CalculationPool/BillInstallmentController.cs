using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    //[Route("v1/bill")]
    //public class BillInstallmentController : BaseController
    //{
    //    private readonly IBillInstallmentHandler _billInstallmentHandler;
    //    public BillInstallmentController(IBillInstallmentHandler billInstallmentHandler)
    //    {
    //        _billInstallmentHandler = billInstallmentHandler;
    //        _billInstallmentHandler.NotNull(nameof(billInstallmentHandler));
    //    }

    //    [HttpPost]
    //    [Route("installment")]
    //    [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto>>), StatusCodes.Status200OK)]
    //    public async Task<IActionResult> Installment([FromBody] InstallmentInputDto inputDto, CancellationToken cancellationToken)
    //    {
    //        ReportOutput<InstallmentHeaderOutputDto, InstallmentDataOutputDto> result = await _billInstallmentHandler.Handle(inputDto, cancellationToken);
    //        return Ok(result);
    //    }
    //}

    [Route("v1/bill-installment")]
    public class BillInstallmentController : BaseController
    {
        private readonly IBillInstallmentCreateHandler _billInstallmentCreateHandler;
        private readonly IBillInstallmentGetHandler _billInstallmentGetHandler;
        public BillInstallmentController(
            IBillInstallmentCreateHandler billInstallmentCreateHandler,
            IBillInstallmentGetHandler billInstallmentGetHandler)
        {
            _billInstallmentCreateHandler = billInstallmentCreateHandler;
            _billInstallmentCreateHandler.NotNull(nameof(billInstallmentCreateHandler));

            _billInstallmentGetHandler = billInstallmentGetHandler;
            _billInstallmentGetHandler.NotNull(nameof(billInstallmentGetHandler));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddInstallment([FromBody] BillInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> result = await _billInstallmentCreateHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }


        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetail([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto> result = await _billInstallmentGetHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(result);
        }
    }
}
