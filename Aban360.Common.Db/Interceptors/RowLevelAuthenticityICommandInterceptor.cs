using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Auditing;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Aban360.Common.Db.Interceptors
{
    public class RowLevelAuthenticitySaveChangeInterceptor :
        SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData?.Context is null)
            {
                return result;
            }
            BeforeSaveTriggers(eventData.Context).Wait();

            return result;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData?.Context is null)
            {
                return result; //ValueTask.FromResult(result);
            }

            await BeforeSaveTriggers(eventData.Context);

            return result;//ValueTask.FromResult(result);
        }

        private async Task BeforeSaveTriggers(DbContext context)
        {
            var hashableEntries = GetHashableEntities(context);
            foreach (var auditEntry in hashableEntries)
            {
                AddShadowProperties(auditEntry);
                await CalculateHash(auditEntry);
            }
        }
        private IList<AuditEntry> GetHashableEntities(DbContext context)
        {
            var auditEntries = new List<AuditEntry>();
            var hashableEntries = context.ChangeTracker.Entries<IHashableEntity>();
            foreach (var entry in hashableEntries)
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }
                var auditEntry = new AuditEntry(entry);
                auditEntries.Add(auditEntry);

                //var now = DateTimeOffset.UtcNow;

                /*foreach (var property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;
                    if (propertyName == nameof(IHashableEntity.Hash) || IgnoreHashNames.Names.Contains(propertyName))
                    {
                        continue;
                    }
                    if (property.IsTemporary)
                    {
                        // It's an auto-generated value and should be retrieved from the DB after calling the base.SaveChanges().
                        auditEntry.AuditProperties.Add(new AuditProperty(propertyName, null, true, property));
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            //entry.Property(AuditableShadowProperties.CreatedDateTime).CurrentValue = now;
                            auditEntry.AuditProperties.Add(new AuditProperty(propertyName, property.CurrentValue, false, property));
                            break;
                        case EntityState.Modified:
                            auditEntry.AuditProperties.Add(new AuditProperty(propertyName, property.CurrentValue, false, property));
                            // entry.Property(AuditableShadowProperties.ModifiedDateTime).CurrentValue = now;
                            break;
                    }
                }*/
            }

            return auditEntries;
        }
        private async Task CalculateHash(AuditEntry auditEntry)
        {
            foreach (var auditProperty in auditEntry.AuditProperties.Where(x => x.IsTemporary))
            {
                // Now we have the auto-generated value from the DB.
                auditProperty.Value = auditProperty.PropertyEntry.CurrentValue;
                auditProperty.IsTemporary = false;
            }
            var dic = auditEntry.AuditProperties.ToDictionary(x => x.Name, x => x.Value);
            auditEntry.EntityEntry.Property(nameof(IHashableEntity.Hash)).CurrentValue =
               await auditEntry.AuditProperties.ToDictionary(x => x.Name, x => x.Value).GenerateObjectHash();
        }
        private void AddShadowProperties(AuditEntry auditEntry)
        {
            switch (auditEntry.EntityEntry.State)
            {
                case EntityState.Added:
                    auditEntry.EntityEntry.Property(nameof(IHashableEntity.ValidFrom)).CurrentValue = DateTime.Now;
                    //auditEntry.AuditProperties.Add(new AuditProperty(propertyName, property.CurrentValue, false, property));
                    break;
                case EntityState.Modified:
                    auditEntry.EntityEntry.Property(nameof(IHashableEntity.ValidTo)).CurrentValue = DateTime.Now;
                    break;
            }
        }
        private void ApplyAudit()
        {

        }
        private void ValidateEntities()
        {

        }
    }
}
