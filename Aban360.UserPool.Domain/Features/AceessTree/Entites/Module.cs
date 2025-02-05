using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.AceessTree.Entites;

[Table(nameof(Module))]
public class Module
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Title { get; set; } = null!;

    public string? Style { get; set; }

    public bool InMenu { get; set; }

    public int LogicalOrder { get; set; }

    public string? ClientRoute { get; set; }

    public bool IsActive { get; set; }

    public virtual App App { get; set; } = null!;

    public virtual ICollection<SubModule> Controllers { get; set; } = new List<SubModule>();
}
