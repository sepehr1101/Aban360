using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class OfferingGroupGetAllHandler : IOfferingGroupGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfferingGroupQueryService _offeringGroupQueryService;
        public OfferingGroupGetAllHandler(
            IMapper mapper,
            IOfferingGroupQueryService offeringGroupQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _offeringGroupQueryService = offeringGroupQueryService;
            _offeringGroupQueryService.NotNull(nameof(offeringGroupQueryService));
        }

        public async Task<ICollection<OfferingGroupGetDto>> Handle(CancellationToken cancellationToken)
        {
            var offeringGroup = await _offeringGroupQueryService.Get();
            if (offeringGroup == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<OfferingGroupGetDto>>(offeringGroup);
        }
    }
}
