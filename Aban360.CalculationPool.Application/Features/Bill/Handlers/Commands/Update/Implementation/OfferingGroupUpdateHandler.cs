using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class OfferingGroupUpdateHandler : IOfferingGroupUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupQueryService _offeringGroupQueryService;
        public OfferingGroupUpdateHandler(
            IMapper mapper,
            IOfferingGroupQueryService offeringGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupQueryService = offeringGroupQueryService;
            _offeringGroupQueryService.NotNull(nameof(offeringGroupQueryService));
        }

        public async Task Handle(OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var offeringGroup = await _offeringGroupQueryService.Get(updateDto.Id);
            if (offeringGroup == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, offeringGroup);
        }
    }
}
