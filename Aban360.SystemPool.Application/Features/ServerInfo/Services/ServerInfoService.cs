using Aban360.Common.Extensions;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using AutoMapper;
using DNTPersianUtils.Core;

namespace Aban360.SystemPool.Application.Features.ServerInfo.Services;

public interface IServerInfoService
{
    OsInfo GetOsInfo();
    DiskInfo[] GetDiskInfo();
}

internal sealed class ServerInfoService : IServerInfoService
{
    private readonly IMapper _mapper;
    public ServerInfoService(IMapper mapper)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(_mapper));
    }

    public OsInfo GetOsInfo()
    {
        var ticks = Environment.TickCount;
        var secondsTotal = ticks / 1000;
        var minTotal = secondsTotal / 60;
        var hoursTotal = minTotal / 60;
        var day = hoursTotal / 24;
        var hours = hoursTotal - day * 24;
        var mins = minTotal - (hoursTotal * 60);
        var seconds = secondsTotal - (minTotal * 60);
        var osInfo = new OsInfo()
        {
            CpuCoreCount = Environment.ProcessorCount,
            IsOs64 = Environment.Is64BitOperatingSystem,
            ServicePack = Environment.OSVersion.ServicePack,
            SystemDateTime = DateTime.Now.ToLongPersianDateTimeString(),
            Version = Environment.OSVersion.VersionString,
            ElapsedDateTime = $"{day} روز، {hours} ساعت، {mins} دقیقه، {seconds} ثانیه"};
        return osInfo;
    }

    public DiskInfo[] GetDiskInfo()
    {
        DriveInfo[] drivesInfo = DriveInfo.GetDrives().Where(d => d.IsReady).ToArray();
        DiskInfo[] disksInfo = _mapper.Map<DiskInfo[]>(drivesInfo);
        return disksInfo;
    }
}
