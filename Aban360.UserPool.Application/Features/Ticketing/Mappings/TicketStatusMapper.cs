using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.UserPool.Domain.Features.Ticketing.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Ticketing.Mappings
{
    public class TicketStatusMapper : Profile
    {
        public TicketStatusMapper()
        {
            CreateMap<TicketStatus, TicketStatusGetDto>();
        }
    }
}
