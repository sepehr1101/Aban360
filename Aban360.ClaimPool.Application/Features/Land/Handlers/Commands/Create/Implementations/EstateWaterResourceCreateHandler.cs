using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class EstateWaterResourceCreateHandler : IEstateWaterResourceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceCommandService _estateWaterResourceCommandService;
        public EstateWaterResourceCreateHandler(
            IMapper mapper,
            IEstateWaterResourceCommandService EstateWaterResourceCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceCommandService = EstateWaterResourceCommandService;
            _estateWaterResourceCommandService.NotNull(nameof(_estateWaterResourceCommandService));

        }

        public async Task Handle(EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            EstateWaterResource estateWaterResource = _mapper.Map<EstateWaterResource>(createDto);
            if (estateWaterResource == null)
            {
                throw new InvalidDataException();
            }
            await _estateWaterResourceCommandService.Add(estateWaterResource);
        }
    }
}
