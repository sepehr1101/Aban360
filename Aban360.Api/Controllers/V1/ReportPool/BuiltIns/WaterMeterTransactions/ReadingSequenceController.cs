using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/reading-sequence")]
    public class ReadingSequenceController : BaseController
    {
        private readonly IReadingSequenceDetailHandler _readingSequence;
        private readonly IReportGenerator _reportGenerator;
        public ReadingSequenceController(
            IReadingSequenceDetailHandler readingSequence,
            IReportGenerator reportGenerator)
        {
            _readingSequence = readingSequence;
            _readingSequence.NotNull(nameof(_readingSequence));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ReadingSequenceHeaderOutputDto, ReadingSequenceDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ReadingSequenceInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ReadingSequenceHeaderOutputDto, ReadingSequenceDataOutputDto> debtorByDay = await _readingSequence.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ReadingSequenceInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _readingSequence.Handle, CurrentUser, ReportLiterals.ReadingSequenceDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
