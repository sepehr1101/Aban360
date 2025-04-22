using Aban360.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;

[Table(nameof(DynamicReport))]
public class DynamicReport:IHashableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public long Version { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public string UserDisplayName { get; set; } = null!;
    public string ReportTemplateJson { get; set; }= null!;
}
