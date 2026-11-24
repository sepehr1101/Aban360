using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.Api.Filters;
using IRequestUsageService = global::Aban360.ClaimPool.Application.Features.Land.Services.IRequestUsageService;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    public class UsageController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly IRequestUsageService _requestUsageService;
        private readonly IUsageHandler _c1_usage;
        private readonly IReportGenerator _c1_reportGenerator;
        public UsageController(IUnitOfWork c0_uow, IRequestUsageService requestUsageService, IUsageHandler c1_usage, IReportGenerator c1_reportGenerator)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(_c0_uow));
            _requestUsageService = requestUsageService;
            _requestUsageService.NotNull(nameof(_requestUsageService));
            _c1_usage = c1_usage;
            _c1_usage.NotNull(nameof(_c1_usage));
            _c1_reportGenerator = c1_reportGenerator;
            _c1_reportGenerator.NotNull(nameof(_c1_reportGenerator));
        }

        [HttpPost]
        [Route("~/v1/usage/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageCreateDto>), StatusCodes.Status200OK)]
        [ActionName("Create")]
        public async Task<IActionResult> CreateFromUsage0([FromBody] UsageCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestUsageService.UsageCreateAsync(createDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/usage/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UsageUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestUsageService.UsageUpdateAsync(updateDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/usage/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UsageDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestUsageService.UsageDeleteAsync(deleteDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/usage/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGetDto>), StatusCodes.Status200OK)]
        [ActionName("Create")]
        public async Task<IActionResult> CreateFromUsage0(short id, CancellationToken cancellationToken)
        {
            UsageGetDto usage = await _requestUsageService.UsageGetSingleAsync(id, cancellationToken);
            return Ok(usage);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/usage/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UsageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<UsageGetDto> usages = await _requestUsageService.UsageGetAllAsync(cancellationToken);
            return Ok(usages);
        }

        [HttpPost]
        [Route("~/v1/usage/raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UsageInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto> c1_usage = await _c1_usage.Handle(inputDto, cancellationToken);
            return Ok(c1_usage);
        }

        [HttpPost]
        [Route("~/v1/usage/excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UsageInputDto inputDto, CancellationToken cancellationToken)
        {
            await _c1_reportGenerator.FireAndInform(inputDto, cancellationToken, _c1_usage.Handle, CurrentUser, ReportLiterals.Usage, connectionId);
            return Ok(inputDto);
        }
    }
}
