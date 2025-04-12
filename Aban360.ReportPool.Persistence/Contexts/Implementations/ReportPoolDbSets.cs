using Aban360.UserPool.Persistence.Scaffold;
using Microsoft.EntityFrameworkCore;

namespace Aban360.ReportPool.Persistence.Contexts.Implementations
{
    public partial class ReportPoolContext
    {
        public virtual DbSet<DynamicReport> DynamicReports { get; set; }
    }
}
