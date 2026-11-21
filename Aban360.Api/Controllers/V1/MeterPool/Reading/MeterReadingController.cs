using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.Api.Filters;
using Aban360.MeterPool.Application.Features.Reading.Services;

namespace Aban360.Api.Controllers.V1.MeterPool.Reading
{
    [Route("v1/meter-reading")]
    public class MeterReadingController : BaseController
    {
        private readonly IReadingListService _readingListService;
        public MeterReadingController(IReadingListService readingListService)
        {
            _readingListService = readingListService;
            _readingListService.NotNull(nameof(readingListService));
        }

        [Route("generate-list")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<GenerateReadingListOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateList([FromBody] GenerateReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            GenerateReadingListOutputDto result = await _readingListService.GenerateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Route("load")]
        [HttpPost, HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReaderCartableOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoadList(CancellationToken cancellationToken)
        {
            ReaderCartableOutputDto result = await _readingListService.GetReaderCartableAsync(CurrentUser, cancellationToken);
            return Ok(result);
        }

        [Route("offload")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MeterReadingListOffLoadInputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OffLoadList(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, CancellationToken cancellationToken)
        {
            await _readingListService.OffloadAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [Route("finish")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Finish(long trackNumber, CancellationToken cancellationToken)
        {
            await _readingListService.FinishAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }

        [Route("finish/{trackNumber}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinsihByQuery(long trackNumber, CancellationToken cancellationToken)
        {
            await _readingListService.FinishAsync(trackNumber, CurrentUser, cancellationToken);
            return Ok(trackNumber);
        }
    }
}
