using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using NetTopologySuite.Index.HPRtree;
using System.Linq;

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


            ICollection<RequestFlat> flats = _mapper.Map<ICollection<RequestFlat>>(createDto.Flats);
            foreach (var item in flats)
            {
                item.RequestEstate = estate;
            }

            ICollection<RequestWaterMeter> requestWaterMeters = new List<RequestWaterMeter>() { requestWaterMeter };
            estate.WaterMeters = requestWaterMeters;
            estate.Flats = flats;


            createDto.WaterMeter.TagIds.ForEach(item =>
            {
                RequestWaterMeterTag requestWaterMeterTag = new RequestWaterMeterTag()
                {
                    InsertLogInfo = "loginfo",
                    Hash = "hash",
                    RequestWaterMeter = requestWaterMeters.First(),
                    WaterMeterTagDefinitionId = item
                };
                requestWaterMeters.First().WaterMeterTags.Add(requestWaterMeterTag);
            });

            int number = 0;
            ICollection<RequestIndividual> individuals = _mapper.Map<ICollection<RequestIndividual>>(createDto.Individuals);
            foreach (var item in individuals)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";

                item.IndividualTypeId = 1;
                item.RequestWaterMeter = requestWaterMeter;

                RequestIndividualEstate requestIndividualEstate = new()
                {
                    RequestEstate = estate,
                    RequestIndividual = item,
                    IndividualEstateRelationTypeId = IndividualEstateRelationEnum.OwnerShip,
                };
                estate.IndividualEstates.Add(requestIndividualEstate);

                var individualTags = createDto.Individuals.ElementAt(number).TagIds.ToList();
                individualTags.ForEach(tagId =>
                {
                    RequestIndividualTag requestIndividualTag = new()
                    {
                        InsertLogInfo = "loginfo",
                        Hash = "hash",
                        RequestIndividual = item,
                        IndividualTagDefinitionId = tagId,
                    };
                    item.IndividualTags.Add(requestIndividualTag);
                });


                number++;
            }



            ICollection<RequestSiphon> siphons = _mapper.Map<ICollection<RequestSiphon>>(createDto.Siphons);
            foreach (var item in siphons)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";

                item.RequestWaterMeter = requestWaterMeter;

                RequestWaterMeterSiphon requestWaterMeterSiphon = new RequestWaterMeterSiphon()
                {
                    // WaterMeterId=1,
                    RequestWaterMeter = requestWaterMeter,
                    RequestSiphon = item
                };
                requestWaterMeter.WaterMeterSiphons.Add(requestWaterMeterSiphon);
            }



            await _requestEstateCommandService.Add(estate);
            //await _requestUserCommandService.Add(requestWaterMeter);
        }
    }
}
