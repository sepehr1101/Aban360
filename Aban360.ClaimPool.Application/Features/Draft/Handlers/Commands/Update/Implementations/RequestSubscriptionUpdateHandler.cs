using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestSubscriptionUpdateHandler : IRequestSubscriptionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IValidator<RequestSubscriptionUpdateDto> _requestValidator;
        public RequestSubscriptionUpdateHandler(
            IMapper mapper,
            IValidator<RequestSubscriptionUpdateDto> requestValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _requestValidator = requestValidator;
            _requestValidator.NotNull(nameof(requestValidator));
        }

        public async Task Handle(RequestSubscriptionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            RequestEstate requestEstate=_mapper.Map<RequestEstate>(updateDto.Estate);

            RequestWaterMeter requestWaterMeter = _mapper.Map<RequestWaterMeter>(updateDto.WaterMeter);
            ICollection<RequestFlat> requestFlats=_mapper.Map<ICollection<RequestFlat>>(updateDto.Flats);
            ICollection<RequestIndividual> requestIndividuals = _mapper.Map<ICollection<RequestIndividual>>(updateDto.Individuals);
            ICollection<RequestSiphon> requestSiphons=_mapper.Map<ICollection<RequestSiphon>>(updateDto.Siphons);   
        }
    }
}