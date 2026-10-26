using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Ticketing.Mappings
{
    public class TicketMapper : Profile
    {
        public TicketMapper()
        {
            CreateMap<TicketCreateDto, Ticket>(MemberList.None);
            CreateMap<TicketUpdateDto, Ticket>(MemberList.None);
            CreateMap<Ticket, TicketGetDto>();
        }
    }
}
