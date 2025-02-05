using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.AceessTree.Entites;

[Table(nameof(SubModule))]
public class SubModule
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Style { get; set; }

    public bool InMenu { get; set; }

    public int LogicalOrder { get; set; }

    public string? ClientRoute { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Endpoint> Endpoints{ get; set; } = new List<Endpoint>();

    public virtual Module Module { get; set; } = null!;
}
