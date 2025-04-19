using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class HandoverCreateHandler : IHandoverCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHandoverCommandService _handoverCommandService;
        public HandoverCreateHandler(
            IMapper mapper,
            IHandoverCommandService handoverCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _handoverCommandService = handoverCommandService;
            _handoverCommandService.NotNull(nameof(_handoverCommandService));
        }

        public async Task Handle(HandoverCreateDto createDto, CancellationToken cancellationToken)
        {
            var handover = _mapper.Map<Handover>(createDto);
            await _handoverCommandService.Add(handover);
        }
    }
}
