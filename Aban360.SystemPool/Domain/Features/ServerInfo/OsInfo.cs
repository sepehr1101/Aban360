namespace Aban360.SystemPool.Domain.Features.ServerInfo
{
    public class OsInfo
    {
        public int CpuCoreCount { get; set; }
        public string Version { get; set; } = default!;
        public string? ServicePack { get; set; }
        public string ElapsedDateTime { get; set; } = default!;
        public bool IsOs64 { get; set; }
        public string SystemDateTime { get; set; }= default!;
    }
}
