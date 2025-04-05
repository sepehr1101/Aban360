using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestSubscriptionCreateHandler : IRequestSubscriptionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserCommandService _requestUserCommandService;
        private readonly IRequestEstateCommandService _requestEstateCommandService;
        public RequestSubscriptionCreateHandler(
            IMapper mapper,
            IRequestUserCommandService requestUserCommandService,
            IRequestEstateCommandService requestEstateCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestUserCommandService = requestUserCommandService;
            _requestUserCommandService.NotNull(nameof(_requestUserCommandService));

            _requestEstateCommandService = requestEstateCommandService;
            _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));
        }

        public async Task Handle(RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken)
        {
            RequestEstate estate = _mapper.Map<RequestEstate>(createDto.Estate);
            estate.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
            estate.InsertLogInfo = "sample";

            RequestWaterMeter requestWaterMeter = _mapper.Map<RequestWaterMeter>(createDto.WaterMeter);
            requestWaterMeter.InsertLogInfo = "loginfo";
            requestWaterMeter.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");


            ICollection<RequestIndividual> individuals = _mapper.Map<ICollection<RequestIndividual>>(createDto.Individuals);
            foreach (var item in individuals)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";

                item.IndividualTypeId = 1;
                item.RequestWaterMeter = requestWaterMeter;
            }


            ICollection<RequestSiphon> siphons = _mapper.Map<ICollection<RequestSiphon>>(createDto.Siphons);
            foreach (var item in siphons)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";
            }

            ICollection<RequestFlat> flats = _mapper.Map<ICollection<RequestFlat>>(createDto.Flats);
            foreach (var item in flats)
            {
                item.RequestEstate = estate;
            }

            ICollection<RequestWaterMeter> requestWaterMeters = new List<RequestWaterMeter>() { requestWaterMeter };
            estate.WaterMeters = requestWaterMeters;
            estate.Flats = flats;



            foreach (var item in individuals)
            {
                RequestIndividualEstate requestIndividualEstate = new()
                {
                    RequestEstate = estate,
                    RequestIndividual = item,
                    IndividualEstateRelationTypeId = IndividualEstateRelationEnum.OwnerShip,
                };
                estate.IndividualEstates.Add(requestIndividualEstate);
            }

            foreach (var item in siphons)
            {
                RequestWaterMeterSiphon requestWaterMeterSiphon = new RequestWaterMeterSiphon()
                {
                    WaterMeterId=1,
                    //RequestWaterMeter = requestWaterMeter,
                    RequestSiphon = item
                };
                requestWaterMeter.WaterMeterSiphons.Add(requestWaterMeterSiphon);
            }

            await _requestEstateCommandService.Add(estate);
            //await _requestUserCommandService.Add(requestWaterMeter);
        }
    }
}
