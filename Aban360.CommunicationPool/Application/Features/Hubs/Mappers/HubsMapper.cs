using Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands;
using AutoMapper;

namespace Aban360.CommunicationPool.Application.Features.Hubs.Mappers
{
    public class HubsMapper:Profile
    {
        public HubsMapper()
        {
            CreateMap<NotifyTextMessageInput, NotifyTextMessageOutput>();
        }
    }
}
