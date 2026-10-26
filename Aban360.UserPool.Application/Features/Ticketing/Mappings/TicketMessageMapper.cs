using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Ticketing.Mappings
{
    public class TicketMessageMapper : Profile
    {
        public TicketMessageMapper()
        {
            CreateMap<TicketMessageCreateDto, TicketMessage>(MemberList.None);
            CreateMap<TicketMessageUpdateDto, TicketMessage>(MemberList.None);
            CreateMap<TicketMessage, TicketMessageGetDto>();
        }
    }
}
