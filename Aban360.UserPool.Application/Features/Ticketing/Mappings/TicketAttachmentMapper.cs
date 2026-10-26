using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Ticketing.Mappings
{
    public class TicketAttachmentMapper : Profile
    {
        public TicketAttachmentMapper()
        {
            CreateMap<TicketFileCreateDto, TicketAttachment>(MemberList.None)
                .ForMember(x => x.SizeBytes, opt => opt.MapFrom(x => x.FileContent.LongLength))
                .ForMember(x => x.FileGuid, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<TicketAttachmentCreateDto, TicketAttachment>(MemberList.None)
                .IncludeBase<TicketFileCreateDto, TicketAttachment>();
            CreateMap<TicketAttachmentUpdateDto, TicketAttachment>(MemberList.None)
                .ForMember(x => x.SizeBytes, opt => opt.MapFrom(x => x.FileContent.LongLength));
            CreateMap<TicketAttachment, TicketAttachmentContentGetDto>();
        }
    }
}
