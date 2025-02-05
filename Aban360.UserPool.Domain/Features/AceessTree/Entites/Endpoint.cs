using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.AceessTree.Entites;

[Table(nameof(Endpoint))]
public class Endpoint
{
    public int Id { get; set; }

    public int SubModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Style { get; set; }

    public bool InMenu { get; set; }

    public int LogicalOrder { get; set; }

    public string? AuthValue { get; set; }

    public bool IsActive { get; set; }


    public virtual SubModule SubModule { get; set; } = null!;
}
