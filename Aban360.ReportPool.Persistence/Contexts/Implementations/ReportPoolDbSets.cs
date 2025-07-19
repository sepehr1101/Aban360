using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;
using Aban360.ReportPool.Domain.Features.FlatReports.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ReportPool.Persistence.Contexts.Implementations
{
    public partial class ReportPoolContext
    {
        public virtual DbSet<DynamicReport> DynamicReports { get; set; }
        public virtual DbSet<ServerReports> ServerReports { get; set; }
    }
}
