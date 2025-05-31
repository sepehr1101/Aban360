using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestSubscriptionCreateHandler : IRequestSubscriptionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestUserCommandService _requestUserCommandService;
        private readonly IRequestEstateCommandService _requestEstateCommandService;
        private readonly IValidator<RequestSubscriptionCreateDto> _requestValidator;
        public RequestSubscriptionCreateHandler(
            IMapper mapper,
            IRequestUserCommandService requestUserCommandService,
            IRequestEstateCommandService requestEstateCommandService,
            IValidator<RequestSubscriptionCreateDto> requestValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestUserCommandService = requestUserCommandService;
            _requestUserCommandService.NotNull(nameof(_requestUserCommandService));

            _requestEstateCommandService = requestEstateCommandService;
            _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));

            _requestValidator = requestValidator;
            _requestValidator.NotNull(nameof(_requestValidator));
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
            if (waterMeterDto.TagIds.IsNullOrEmpty())
            {
                return;
            }
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
        private RequestEstate GetEstate(IAppUser currentUser, EstateRequestCreateDto estateDto)
        {
            RequestEstate estate = _mapper.Map<RequestEstate>(estateDto);
            estate.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
            estate.InsertLogInfo = "sample";
            estate.UserId = currentUser.UserId;

            return estate;
        }
        private ICollection<RequestWaterMeter> GetWaterMeter(IAppUser currentUser, WaterMeterRequestCreateDto waterMeterDto, RequestEstate estate)
        {
            RequestWaterMeter requestWaterMeter = _mapper.Map<RequestWaterMeter>(waterMeterDto);
            requestWaterMeter.InsertLogInfo = "loginfo";
            requestWaterMeter.UserId = currentUser.UserId;

            ICollection<RequestWaterMeter> requestWaterMeters = new List<RequestWaterMeter>() { requestWaterMeter };
            estate.WaterMeters = requestWaterMeters;
            return requestWaterMeters;
        }
        private void GetIndividualsAndIndividualEstate(IAppUser currentUser, ICollection<IndividualRequestCreateDto> individualDto, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter)
        {
            ICollection<RequestIndividual> individuals = _mapper.Map<ICollection<RequestIndividual>>(individualDto);
            int number = 0;
            foreach (var item in individuals)
            {
                GetIndividual(currentUser, item, requestEstatestate, requestWaterMeter, individualDto.ElementAt(number));

                var individualTags = individualDto.ElementAt(number).TagIds.ToList();
                GetIndividualTags(number, individualTags, item);

                number++;
            }
        }

        private void GetIndividual(IAppUser currentUser, RequestIndividual individual, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter, IndividualRequestCreateDto individualDto)
        {
            individual.InsertLogInfo = "loginfo";
            individual.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
            individual.Hash = "hash";
            individual.UserId = currentUser.UserId;

            individual.IndividualTypeId = individualDto.IndividualTypeId;

            RequestIndividualEstate requestIndividualEstate = new()
            {
                RequestEstate = requestEstatestate,
                RequestIndividual = individual,
                IndividualEstateRelationTypeId = individualDto.IndividualEstateRelationTypeId,
            };
            individual.IndividualDiscountTypes.ForEach(discountType =>
            {
                discountType.Individual = individual;
                discountType.UserId=currentUser.UserId;
                discountType.Hash = "-";
                discountType.InsertLogInfo = "-";
                discountType.ValidFrom= DateTime.Now;

                //RequestIndividualDiscountType requestIndividualDiscountType = new RequestIndividualDiscountType()
                //{
                //    Individual = individual,
                //    DiscountTypeId = discountType.DiscountTypeId,
                //    ExpireDate = discountType.ExpireDate,
                //    Hash="-",
                //    InsertLogInfo="-",
                //    ValidFrom=DateTime.Now,
                //    UserId=currentUser.UserId
                //};
                //individual.IndividualDiscountTypes.Add(requestIndividualDiscountType);
            });

            requestEstatestate.IndividualEstates.Add(requestIndividualEstate);

        }
        private void GetIndividualTags(int number, List<short> individualTagsId, RequestIndividual requestIndividual)
        {
            individualTagsId.ForEach(tagId =>
            {
                if (tagId == 0) return;
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
        private void GetSiphon(IAppUser currentUser, ICollection<SiphonRequestCreateDto> siphonDto, RequestWaterMeter requestWaterMeter)
        {
            ICollection<RequestSiphon> siphons = _mapper.Map<ICollection<RequestSiphon>>(siphonDto);
            foreach (var item in siphons)
            {
                item.InsertLogInfo = "loginfo";
                item.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
                item.Hash = "hash";
                item.UserId = currentUser.UserId;

                RequestWaterMeterSiphon requestWaterMeterSiphon = new RequestWaterMeterSiphon()
                {
                    RequestWaterMeter = requestWaterMeter,
                    RequestSiphon = item
                };
                requestWaterMeter.WaterMeterSiphons.Add(requestWaterMeterSiphon);
            }
        }

        public async Task Handle(IAppUser currentUser, RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _requestValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }



            RequestEstate estate = GetEstate(currentUser, createDto.Estate);
            ICollection<RequestWaterMeter> requestWaterMeters = GetWaterMeter(currentUser, createDto.WaterMeter, estate);
            Random random = new Random();
            requestWaterMeters.ToList().ForEach(waterMeter =>
            {
                waterMeter.TrackNumber = random.Next(1, 1000001).ToString("D6");
            });

            GetFlats(createDto.Flats, estate);
            GetWaterMeterTags(createDto.WaterMeter, requestWaterMeters.First());
            GetIndividualsAndIndividualEstate(currentUser, createDto.Individuals, estate, requestWaterMeters.First());
            GetSiphon(currentUser, createDto.Siphons, requestWaterMeters.First());



            await _requestEstateCommandService.Add(estate);
        }


    }
}