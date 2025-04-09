using Aban360.SystemPool.Domain.Features.ServerInfo;
using AutoMapper;

namespace Aban360.SystemPool.Application.Features.ServerInfo.Mappings
{
    public class DiskInfoMapper:Profile
    {
        public DiskInfoMapper()
        {
            CreateMap<DriveInfo,DiskInfo>()
                .ForMember(dest => dest.DriveName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.VolumeLabel, opt => opt.MapFrom(src => src.VolumeLabel))
                .ForMember(dest => dest.FreeSpaceGb, opt => opt.MapFrom(src => src.TotalFreeSpace/ 1073741824))
                .ForMember(dest => dest.TotalSpaceGb, opt => opt.MapFrom(src => src.TotalSize/ 1073741824))
                .ForMember(dest => dest.AvaialableSpaceGb, opt => opt.MapFrom(src => src.AvailableFreeSpace))
                .ForMember(dest => dest.ReservedSpaceMb, opt => opt.MapFrom(src => (src.TotalFreeSpace-src.AvailableFreeSpace)/ 1048576));
        }
    }
}
