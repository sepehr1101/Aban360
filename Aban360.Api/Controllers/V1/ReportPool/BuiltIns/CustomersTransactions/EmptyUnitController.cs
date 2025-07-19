using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Excel;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Application.Features.FlatReports.Background.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/empty-unit")]
    public class EmptyUnitController : BaseController
    {
        private readonly IEmptyUnitHandler _emptyUnit;
        private readonly IBackgroundExcel _background;
        public EmptyUnitController(
            IEmptyUnitHandler emptyUnit,
            IBackgroundExcel background)
        {
            _emptyUnit = emptyUnit;
            _emptyUnit.NotNull(nameof(_emptyUnit));

            _background = background;
            _background.NotNull(nameof(_background));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel")]
        public async Task<IActionResult> GetExcel(EmptyUnitInputDto inputDto, CancellationToken cancellationToken)
        {
           // ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnit.Handle(inputDto, cancellationToken);
            // var result = await ExcelManagement.ExportToExcelAsync(emptyUnit.ReportHeader, emptyUnit.ReportData, emptyUnit.Title);
         
            var result = await _background.Background<IEmptyUnitHandler, EmptyUnitHeaderOutputDto
                                                    ,EmptyUnitDataOutputDto,EmptyUnitInputDto>(_emptyUnit, inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
