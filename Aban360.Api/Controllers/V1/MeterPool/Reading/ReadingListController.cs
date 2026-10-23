using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class ReadingListController : BaseController
    {
        private readonly IReaderCartableGetHandler _readerCartableGetHandler;
        private readonly IMeterReadingListOffLoadHandler _meterReadingListOffLoadHandler;
        private readonly IMeterReadingListFinishHandler _meterReadingListFinishHandler;
        public ReadingListController(
            IReaderCartableGetHandler readerCartableGetHandler,
            IMeterReadingListOffLoadHandler meterReadingListOffLoadHandler,
            IMeterReadingListFinishHandler meterReadingListFinishHandler)
        {
            _readerCartableGetHandler = readerCartableGetHandler;
            _readerCartableGetHandler.NotNull(nameof(readerCartableGetHandler));

            _meterReadingListOffLoadHandler = meterReadingListOffLoadHandler;
            _meterReadingListOffLoadHandler.NotNull(nameof(meterReadingListOffLoadHandler));

            _meterReadingListFinishHandler = meterReadingListFinishHandler;
            _meterReadingListFinishHandler.NotNull(nameof(meterReadingListFinishHandler));
        }

        [Route("load")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReaderCartableOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoadList(CancellationToken cancellationToken)
        {
            ReaderCartableOutputDto result = await _readerCartableGetHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Route("offload")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingListOffLoadInputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OffLoadList(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, CancellationToken cancellationToken)
        {
            await _meterReadingListOffLoadHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [Route("finish")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Finsih(long trackNumber, CancellationToken cancellationToken)
        {
            await _meterReadingListFinishHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }

        [Route("finish/{trackNumber}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinsihByQuery(long trackNumber, CancellationToken cancellationToken)
        {
            await _meterReadingListFinishHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
    }
}
