using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Ticketing.Mappings
{
    public class TicketRatingMapper : Profile
    {
        public TicketRatingMapper()
        {
            CreateMap<TicketRatingCreateDto, TicketRating>(MemberList.None);
            CreateMap<TicketRatingUpdateDto, TicketRating>(MemberList.None);
            CreateMap<TicketRating, TicketRatingGetDto>();
        }
    }
}
