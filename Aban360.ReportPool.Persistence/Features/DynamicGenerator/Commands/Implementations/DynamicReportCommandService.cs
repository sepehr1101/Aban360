using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Commands.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ReportPool.Persistence.Features.DynamicGenerator.Commands.Implementations
{
    internal sealed class DynamicReportCommandService : IDynamicReportCommandService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DynamicReport> _dynamicReport;
        public DynamicReportCommandService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _dynamicReport = _uow.Set<DynamicReport>();
            _dynamicReport.NotNull(nameof(_dynamicReport));
        }

        public async Task Add(DynamicReport dynamicReport)
        {
            await _dynamicReport.AddAsync(dynamicReport);
        }

        public async Task Remove(DynamicReport dynamicReport)
        {
            _dynamicReport.Remove(dynamicReport);
        }
    }
}
