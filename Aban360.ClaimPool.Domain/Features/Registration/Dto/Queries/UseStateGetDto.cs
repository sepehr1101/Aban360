using Aban360.ClaimPool.Domain.Features.Registration.Entities;

namespace Aban360.ClaimPool.Domain.Features.Registration.Dto.Queries
{
    public record UseStateGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
