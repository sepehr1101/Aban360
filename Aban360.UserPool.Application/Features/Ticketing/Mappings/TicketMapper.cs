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
            CreateMap<Ticket, TicketGetDto>()
                .ForMember(dto => dto.CreatedByUserFullName, options => options.MapFrom(ticket =>
                    ticket.CreatedByUser != null ? ticket.CreatedByUser.FullName : null))
                .ForMember(dto => dto.CreatedByUserRoles, options => options.MapFrom(ticket =>
                    ticket.CreatedByUser != null
                        ? string.Join(", ", ticket.CreatedByUser.UserRoles
                            .Where(userRole => userRole.ValidTo == null)
                            .Select(userRole => userRole.Role.Title)
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(title => title, StringComparer.OrdinalIgnoreCase))
                        : string.Empty));
        }
    }
}
