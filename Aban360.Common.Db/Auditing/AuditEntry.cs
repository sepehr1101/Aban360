using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aban360.Common.Db.Auditing
{
    public class AuditEntry
    {
        public EntityEntry EntityEntry { set; get; }
        public IList<AuditProperty> AuditProperties { set; get; } = new List<AuditProperty>();

        public AuditEntry() { }

        public AuditEntry(EntityEntry entry)
        {
            EntityEntry = entry;
        }
    }
}