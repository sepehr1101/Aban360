using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class EstateCreateHandler : IEstateCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateCommandService _commandService;
        public EstateCreateHandler(
            IMapper mapper,
            IEstateCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(_commandService));
        }

        public async Task Handle(EstateCreateDto createDto, CancellationToken cancellationToken)
        {
            Estate estate = _mapper.Map<Estate>(createDto);
            estate.ValidFrom = DateTime.Now;
            estate.InsertLogInfo = "loginfo";
            estate.Hash = "hash";

            await _commandService.Add(estate);
        }
    }
}
