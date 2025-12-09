using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.TaxPool.Application.Features.MaaherSTP.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.TaxPool.MaaherTSP.Queries
{
    [Route("v1/tax")]
    public class TaxExcelController : BaseController
    {
        private readonly IMaliatMaaherDetailGetByWrapperIdHandler _maliatMaaherDetailGetHandler;
        private readonly IReportGenerator _reportGenerator;
        public TaxExcelController(
            IMaliatMaaherDetailGetByWrapperIdHandler maliatMaaherDetailGetHandler,
            IReportGenerator reportGenerator)
        {
            _maliatMaaherDetailGetHandler = maliatMaaherDetailGetHandler;
            _maliatMaaherDetailGetHandler.NotNull(nameof(maliatMaaherDetailGetHandler));
        
            _reportGenerator= reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));  
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId,SearchInput inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _maliatMaaherDetailGetHandler.Handle, CurrentUser, ReportLiterals.ClientValidation, connectionId);
            return Ok(inputDto);
        }

    }
}
