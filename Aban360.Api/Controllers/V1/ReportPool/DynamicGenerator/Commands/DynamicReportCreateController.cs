using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Report = Aban360.ReportPool.Persistence.Contexts.Contracts;
using Blob = Aban360.BlobPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Commands
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportCreateController : BaseController
    {
        private readonly Report.IUnitOfWork _reportUow;
        private readonly Blob.IUnitOfWork _blobUow;
        private readonly IDynamicReportCreateHandler _dynamicReportCreateHandler;
        public DynamicReportCreateController(
            Report.IUnitOfWork reportUow,
            Blob.IUnitOfWork blobUow,
            IDynamicReportCreateHandler tariffConstantCreateHandler)
        {
            _reportUow = reportUow;
            _reportUow.NotNull(nameof(reportUow));

            _blobUow = blobUow;
            _blobUow.NotNull(nameof(blobUow));

            _dynamicReportCreateHandler = tariffConstantCreateHandler;
            _dynamicReportCreateHandler.NotNull(nameof(tariffConstantCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DynamicReportCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DynamicReportCreateDto createDto, CancellationToken cancellationToken)
        {
            IAppUser currentUser = CurrentUser;
            await _dynamicReportCreateHandler.Handle(currentUser, createDto, cancellationToken);

            using (var transaction = TransactionBuilder.Create(1, isolationLevel:IsolationLevel.ReadCommitted))
            {
                await _reportUow.SaveChangesAsync(cancellationToken);
                await _blobUow.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            return Ok(createDto);
        }
    }
}
