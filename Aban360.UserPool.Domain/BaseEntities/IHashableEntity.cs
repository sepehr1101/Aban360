namespace Aban360.UserPool.Domain.BaseEntities
{
    public class IHashableEntity
    {
        public string Hash { get; set; } = default!;
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
        public string? RemoveLogInfo { get; set; }
    }
}