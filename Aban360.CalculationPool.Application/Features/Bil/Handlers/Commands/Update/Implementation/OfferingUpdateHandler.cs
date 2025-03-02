using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Implementation
{
    public class OfferingUpdateHandler : IOfferingUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingQueryService _offeringQueryService;
        public OfferingUpdateHandler(
            IMapper mapper,
            IOfferingQueryService offeringQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }

        public async Task Handle(OfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var offering = await _offeringQueryService.Get(updateDto.Id);
            if (offering == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, offering);
        }
    }
}
