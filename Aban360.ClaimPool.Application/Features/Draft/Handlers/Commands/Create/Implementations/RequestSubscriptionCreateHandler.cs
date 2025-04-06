using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;
using NetTopologySuite.Index.HPRtree;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private void GetFlats(ICollection<FlatRequestCreateDto> flats, RequestEstate estate)
        {
            ICollection<RequestFlat> requestFlats = _mapper.Map<ICollection<RequestFlat>>(flats);
            foreach (var item in requestFlats)
                item.RequestEstate = estate;

            estate.Flats = requestFlats;
        }
        private void GetWaterMeterTags(WaterMeterRequestCreateDto waterMeterDto, RequestWaterMeter requestWaterMeter)
        {
            waterMeterDto.TagIds.ForEach(item =>
            {
                RequestWaterMeterTag requestWaterMeterTag = new RequestWaterMeterTag()
                {
                    InsertLogInfo = "loginfo",
                    Hash = "hash",
                    RequestWaterMeter = requestWaterMeter,
                    WaterMeterTagDefinitionId = item
                };
                requestWaterMeter.WaterMeterTags.Add(requestWaterMeterTag);
            });
        }
        private RequestEstate GetEstate(EstateRequestCreateDto estateDto)
        {
            RequestEstate estate = _mapper.Map<RequestEstate>(estateDto);
            estate.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
            estate.InsertLogInfo = "sample";

            return estate;
        }
        private ICollection<RequestWaterMeter> GetWaterMeter(WaterMeterRequestCreateDto waterMeterDto, RequestEstate estate)
        {
            RequestWaterMeter requestWaterMeter = _mapper.Map<RequestWaterMeter>(waterMeterDto);
            requestWaterMeter.InsertLogInfo = "loginfo";
            requestWaterMeter.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");

            ICollection<RequestWaterMeter> requestWaterMeters = new List<RequestWaterMeter>() { requestWaterMeter };
            estate.WaterMeters = requestWaterMeters;
            return requestWaterMeters;
        }
        private void GetIndividualsAndIndividualEstate(ICollection<IndividualRequestCreateDto> individualDto, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter)
        {
            ICollection<RequestIndividual> individuals = _mapper.Map<ICollection<RequestIndividual>>(individualDto);
            int number = 0;
            foreach (var item in individuals)
            {
                GetIndividual(item, requestEstatestate, requestWaterMeter,individualDto.ElementAt(number));
              
                var individualTags = individualDto.ElementAt(number).TagIds.ToList();
                GetIndividualTags(number, individualTags, item);

                number++;
            }
        }
       
        private void GetIndividual(RequestIndividual individual, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter, IndividualRequestCreateDto individualDto)
        {
            individual.InsertLogInfo = "loginfo";
            individual.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
            individual.Hash = "hash";

            individual.IndividualTypeId = individualDto.IndividualTypeId;
            individual.RequestWaterMeter = requestWaterMeter;

            RequestIndividualEstate requestIndividualEstate = new()
            {
                RequestEstate = requestEstatestate,
                RequestIndividual = individual,
                IndividualEstateRelationTypeId = individualDto.IndividualEstateRelationTypeId,
            };
            requestEstatestate.IndividualEstates.Add(requestIndividualEstate);

        }
        private void GetIndividualTags(int number, List<short> individualTagsId, RequestIndividual requestIndividual)
        {
            individualTagsId.ForEach(tagId =>
            {
                RequestIndividualTag requestIndividualTag = new()
                {
                    InsertLogInfo = "loginfo",
                    Hash = "hash",
                    RequestIndividual = requestIndividual,
                    IndividualTagDefinitionId = tagId,
                };
                requestIndividual.IndividualTags.Add(requestIndividualTag);
            });
        }
        private void GetSiphon(ICollection<SiphonRequestCreateDto> siphonDto, RequestWaterMeter requestWaterMeter)
        {
            ICollection<RequestSiphon> siphons = _mapper.Map<ICollection<RequestSiphon>>(siphonDto);
            foreach (var item in siphons)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";

                item.RequestWaterMeter = requestWaterMeter;

                RequestWaterMeterSiphon requestWaterMeterSiphon = new RequestWaterMeterSiphon()
                {
                    RequestWaterMeter = requestWaterMeter,
                    RequestSiphon = item
                };
                requestWaterMeter.WaterMeterSiphons.Add(requestWaterMeterSiphon);
            }
        }
        public async Task Handle(RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken)
        {
            RequestEstate estate = GetEstate(createDto.Estate);
            ICollection<RequestWaterMeter> requestWaterMeters = GetWaterMeter(createDto.WaterMeter, estate);

            GetFlats(createDto.Flats, estate);
            GetWaterMeterTags(createDto.WaterMeter, requestWaterMeters.First());
            GetIndividualsAndIndividualEstate(createDto.Individuals, estate, requestWaterMeters.First());
            GetSiphon(createDto.Siphons, requestWaterMeters.First());



            await _requestEstateCommandService.Add(estate);
        }
    }
}