using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    public class MeterReadingController : BaseController
    {
        private readonly IGenerateReadingListCreateHandler _c0_generateReadingListCreateHandler;
        private readonly IReaderCartableGetHandler _c1_readerCartableGetHandler;
        private readonly IMeterReadingListOffLoadHandler _c1_meterReadingListOffLoadHandler;
        private readonly IMeterReadingListFinishHandler _c1_meterReadingListFinishHandler;
        public MeterReadingController(IGenerateReadingListCreateHandler c0_generateReadingListCreateHandler, IReaderCartableGetHandler c1_readerCartableGetHandler, IMeterReadingListOffLoadHandler c1_meterReadingListOffLoadHandler, IMeterReadingListFinishHandler c1_meterReadingListFinishHandler)
        {
            _c0_generateReadingListCreateHandler = c0_generateReadingListCreateHandler;
            _c0_generateReadingListCreateHandler.NotNull(nameof(c0_generateReadingListCreateHandler));
            _c1_readerCartableGetHandler = c1_readerCartableGetHandler;
            _c1_readerCartableGetHandler.NotNull(nameof(c1_readerCartableGetHandler));
            _c1_meterReadingListOffLoadHandler = c1_meterReadingListOffLoadHandler;
            _c1_meterReadingListOffLoadHandler.NotNull(nameof(c1_meterReadingListOffLoadHandler));
            _c1_meterReadingListFinishHandler = c1_meterReadingListFinishHandler;
            _c1_meterReadingListFinishHandler.NotNull(nameof(c1_meterReadingListFinishHandler));
        }

        [Route("~/v1/meter-reading/generate-list")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GenerateReadingListOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateList([FromBody] GenerateReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            GenerateReadingListOutputDto result = await _c0_generateReadingListCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Route("~/v1/meter-reading/load")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReaderCartableOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoadList(CancellationToken cancellationToken)
        {
            ReaderCartableOutputDto result = await _c1_readerCartableGetHandler.Handle(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Route("~/v1/meter-reading/offload")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingListOffLoadInputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OffLoadList(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, CancellationToken cancellationToken)
        {
            await _c1_meterReadingListOffLoadHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [Route("~/v1/meter-reading/finish")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Finish(long trackNumber, CancellationToken cancellationToken)
        {
            await _c1_meterReadingListFinishHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }

        [Route("~/v1/meter-reading/finish/{trackNumber}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinsihByQuery(long trackNumber, CancellationToken cancellationToken)
        {
            await _c1_meterReadingListFinishHandler.Handle(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
    }
}
