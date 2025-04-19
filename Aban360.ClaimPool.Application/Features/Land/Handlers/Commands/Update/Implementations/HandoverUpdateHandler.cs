using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class HandoverUpdateHandler : IHandoverUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IHandoverQueryService _handoverQueryService;
        public HandoverUpdateHandler(
            IMapper mapper,
            IHandoverQueryService handoverQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _handoverQueryService = handoverQueryService;
            _handoverQueryService.NotNull(nameof(_handoverQueryService));
        }

        public async Task Handle(HandoverUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var handover = await _handoverQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, handover);
        }
    }
}
