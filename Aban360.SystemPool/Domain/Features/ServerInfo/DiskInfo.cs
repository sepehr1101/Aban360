namespace Aban360.SystemPool.Domain.Features.ServerInfo
{
    public class DiskInfo
    {
        public string DriveName { get; set; } = default!;
        public string VolumeLabel { get; set; } = default!;
        public double FreeSpaceGb { get; set; }
        public double TotalSpaceGb { get; set; }
        public double UsedSpaceGb { get { return TotalSpaceGb - FreeSpaceGb; } }
        public double AvaialableSpaceGb { get; set; }
        public double ReservedSpaceMb { get; set; }
        public double FreePercent { get { return Math.Round((FreeSpaceGb / TotalSpaceGb) * 100, 1); } }
        public double UsedPercent { get { return Math.Round(100 - FreePercent, 1); } }
    }
}
