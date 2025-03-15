using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class OfferingGroupCreateHandler : IOfferingGroupCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupCommandService _offeringGroupCommandService;
        public OfferingGroupCreateHandler(
            IMapper mapper,
            IOfferingGroupCommandService offeringGroupCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupCommandService = offeringGroupCommandService;
            _offeringGroupCommandService.NotNull(nameof(offeringGroupCommandService));
        }

        public async Task Handle(OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            OfferingGroup offeringGroup = _mapper.Map<OfferingGroup>(createDto);
            await _offeringGroupCommandService.Add(offeringGroup);
        }
    }
}
