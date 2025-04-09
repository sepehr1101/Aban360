using Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using DNTPersianUtils.Core;

namespace Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Implementations
{
    internal sealed class OsInfoHandler : IOsInfoHandler
    {
        public OsInfo Handle()
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
                ElapsedDateTime = $"{day} روز، {hours} ساعت، {mins} دقیقه، {seconds} ثانیه"
            };
            return osInfo;
        }
    }
}
