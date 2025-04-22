using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Implementations
{
    internal sealed class DynamicReportQueryService : IDynamicReportQueryService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<DynamicReport> _dynamicReport;
        public DynamicReportQueryService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _dynamicReport = _uow.Set<DynamicReport>();
            _dynamicReport.NotNull(nameof(_dynamicReport));
        }

        public async Task<DynamicReport> Get(int id)
        {
            return await _uow.FindOrThrowAsync<DynamicReport>(id);
        }

        public async Task<string> GetTemplateJson(int id)
        {
            return await
                _dynamicReport
                .Select(dynamicReport => dynamicReport.ReportTemplateJson)
                .SingleAsync();
        }

        public async Task<ICollection<DynamicReportMasterDto>> GetMasters()
        {
            return await _dynamicReport
                .Select(dynamicReport => new DynamicReportMasterDto()
                {
                    Id = dynamicReport.Id,
                    Description = dynamicReport.Description,
                    Name = dynamicReport.Name,
                    UserDisplayName = dynamicReport.UserDisplayName,
                    Version = dynamicReport.Version,
                })
                .ToListAsync();
        }
    }
}
