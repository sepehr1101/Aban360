using Aban360.Common.Extensions;
using Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts;
using Aban360.SystemPool.Domain.Features.ServerInfo;
using AutoMapper;

namespace Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Implementations
{
    internal sealed class DiskInfoHandler : IDiskInfoHandler
    {
        private readonly IMapper _mapper;
        public DiskInfoHandler(IMapper mapper)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));
        }
        public DiskInfo[] Handle()
        {
            DriveInfo[] drivesInfo = DriveInfo.GetDrives().Where(d => d.IsReady).ToArray();
            DiskInfo[] disksInfo = _mapper.Map<DiskInfo[]>(drivesInfo);
            return disksInfo;
        }
    }
}
