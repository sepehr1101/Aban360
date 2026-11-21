using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Aban360.ClaimPool.Application.Features.Draft.Services;
public interface IRequestSubscriptionService
{
    Task CreateAsync(IAppUser currentUser, RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken);
    Task<RequestSubscriptionGetDto> GetAsync(string billId, CancellationToken cancellationToken);
}

internal sealed class RequestSubscriptionService : IRequestSubscriptionService
{
    private readonly IMapper _mapper;
    private readonly IRequestUserCommandService _requestUserCommandService;
    private readonly IRequestEstateCommandService _requestEstateCommandService;
    private readonly IValidator<RequestSubscriptionCreateDto> _requestValidator;
    private readonly IRequestWaterMeterQueryService _waterMeterQueryService;
    private readonly IRequestEstateQueryService _estateQueryService;
    private readonly IRequestFlatQueryService _flatQueryService;
    private readonly IRequestWaterMeterSiphonQueryService _waterMeterSiphonQueryService;
    private readonly IRequestIndividualEstateQueryService _individualEstateQueryService;
    private readonly IRequestIndividualTagQueryService _individualTagQueryService;
    private readonly IRequestIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
    public RequestSubscriptionService(IMapper mapper, IRequestUserCommandService requestUserCommandService, IRequestEstateCommandService requestEstateCommandService, IValidator<RequestSubscriptionCreateDto> requestValidator, IRequestWaterMeterQueryService waterMeterQueryService, IRequestEstateQueryService estateQueryService, IRequestFlatQueryService flatQueryService, IRequestWaterMeterSiphonQueryService waterMeterSiphonQueryService, IRequestIndividualEstateQueryService individualEstateQueryService, IRequestIndividualTagQueryService individualTagQueryService, IRequestIndividualDiscountTypeQueryService individualDiscountTypeQueryService)
    {
        _mapper = mapper;
        _mapper.NotNull(nameof(_mapper));
        _requestUserCommandService = requestUserCommandService;
        _requestUserCommandService.NotNull(nameof(_requestUserCommandService));
        _requestEstateCommandService = requestEstateCommandService;
        _requestEstateCommandService.NotNull(nameof(_requestEstateCommandService));
        _requestValidator = requestValidator;
        _requestValidator.NotNull(nameof(_requestValidator));
        _waterMeterQueryService = waterMeterQueryService;
        _waterMeterQueryService.NotNull(nameof(waterMeterQueryService));
        _estateQueryService = estateQueryService;
        _estateQueryService.NotNull(nameof(estateQueryService));
        _flatQueryService = flatQueryService;
        _flatQueryService.NotNull(nameof(flatQueryService));
        _waterMeterSiphonQueryService = waterMeterSiphonQueryService;
        _waterMeterSiphonQueryService.NotNull(nameof(waterMeterSiphonQueryService));
        _individualEstateQueryService = individualEstateQueryService;
        _individualEstateQueryService.NotNull(nameof(individualEstateQueryService));
        _individualTagQueryService = individualTagQueryService;
        _individualTagQueryService.NotNull(nameof(individualTagQueryService));
        _individualDiscountTypeQueryService = individualDiscountTypeQueryService;
        _individualDiscountTypeQueryService.NotNull(nameof(individualDiscountTypeQueryService));
        _mapper.NotNull(nameof(mapper));
    }

    private void CreateGetFlats(ICollection<FlatRequestCreateDto> flats, RequestEstate estate)
    {
        ICollection<RequestFlat> requestFlats = _mapper.Map<ICollection<RequestFlat>>(flats);
        foreach (var item in requestFlats)
            item.RequestEstate = estate;
        estate.Flats = requestFlats;
    }

    private void CreateGetWaterMeterTags(WaterMeterRequestCreateDto waterMeterDto, RequestWaterMeter requestWaterMeter)
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

    private RequestEstate CreateGetEstate(IAppUser currentUser, EstateRequestCreateDto estateDto)
    {
        RequestEstate estate = _mapper.Map<RequestEstate>(estateDto);
        estate.UserId = Guid.Parse("7DE99BA5-024F-47DA-AACE-23AB662D619C");
        estate.InsertLogInfo = "sample";
        estate.UserId = currentUser.UserId;
        return estate;
    }

    private ICollection<RequestWaterMeter> CreateGetWaterMeter(IAppUser currentUser, WaterMeterRequestCreateDto waterMeterDto, RequestEstate estate)
    {
        RequestWaterMeter requestWaterMeter = _mapper.Map<RequestWaterMeter>(waterMeterDto);
        requestWaterMeter.InsertLogInfo = "loginfo";
        requestWaterMeter.UserId = currentUser.UserId;
        ICollection<RequestWaterMeter> requestWaterMeters = new List<RequestWaterMeter>()
        {
            requestWaterMeter
        };
        estate.WaterMeters = requestWaterMeters;
        return requestWaterMeters;
    }

    private void CreateGetIndividualsAndIndividualEstate(IAppUser currentUser, ICollection<IndividualRequestCreateDto> individualDto, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter)
    {
        ICollection<RequestIndividual> individuals = _mapper.Map<ICollection<RequestIndividual>>(individualDto);
        int number = 0;
        foreach (var item in individuals)
        {
            CreateGetIndividual(currentUser, item, requestEstatestate, requestWaterMeter, individualDto.ElementAt(number));
            var individualTags = individualDto.ElementAt(number).TagIds.ToList();
            CreateGetIndividualTags(number, individualTags, item);
            number++;
        }
    }

    private void CreateGetIndividual(IAppUser currentUser, RequestIndividual individual, RequestEstate requestEstatestate, RequestWaterMeter requestWaterMeter, IndividualRequestCreateDto individualDto)
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
            discountType.UserId = currentUser.UserId;
            discountType.Hash = "-";
            discountType.InsertLogInfo = "-";
            discountType.ValidFrom = DateTime.Now;
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

    private void CreateGetIndividualTags(int number, List<short> individualTagsId, RequestIndividual requestIndividual)
    {
        individualTagsId.ForEach(tagId =>
        {
            if (tagId == 0)
                return;
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

    private void CreateGetSiphon(IAppUser currentUser, ICollection<SiphonRequestCreateDto> siphonDto, RequestWaterMeter requestWaterMeter)
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

    public async Task CreateAsync(IAppUser currentUser, RequestSubscriptionCreateDto createDto, CancellationToken cancellationToken)
    {
        var validationResult = await _requestValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new BaseException(message);
        }

        RequestEstate estate = CreateGetEstate(currentUser, createDto.Estate);
        ICollection<RequestWaterMeter> requestWaterMeters = CreateGetWaterMeter(currentUser, createDto.WaterMeter, estate);
        Random random = new Random();
        requestWaterMeters.ToList().ForEach(waterMeter =>
        {
            waterMeter.TrackNumber = random.Next(1, 1000001).ToString("D6");
        });
        CreateGetFlats(createDto.Flats, estate);
        CreateGetWaterMeterTags(createDto.WaterMeter, requestWaterMeters.First());
        CreateGetIndividualsAndIndividualEstate(currentUser, createDto.Individuals, estate, requestWaterMeters.First());
        CreateGetSiphon(currentUser, createDto.Siphons, requestWaterMeters.First());
        await _requestEstateCommandService.Add(estate);
    }

    public async Task<RequestSubscriptionGetDto> GetAsync(string billId, CancellationToken cancellationToken)
    {
        RequestWaterMeter waterMeter = await _waterMeterQueryService.Get(billId);
        RequestWaterMeterGetDto waterMeterGetDto = _mapper.Map<RequestWaterMeterGetDto>(waterMeter); //todo: mapper
        RequestEstate estate = await _estateQueryService.Get(waterMeter.EstateId);
        RequestEstateGetDto estatesGetDto = _mapper.Map<RequestEstateGetDto>(estate);
        ICollection<RequestFlat> flats = await _flatQueryService.GetByEstateId(waterMeter.EstateId);
        ICollection<RequestFlatGetDto> flastsGetDto = _mapper.Map<ICollection<RequestFlatGetDto>>(flats);
        ICollection<RequestWaterMeterSiphon> waterMeterSiphons = await _waterMeterSiphonQueryService.GetByWaterMeterId(waterMeter.Id);
        ICollection<RequestSiphonGetDto> siphonsGetDto = waterMeterSiphons.Select(watermetersiphon => new RequestSiphonGetDto() { SiphonTypeId = watermetersiphon.RequestSiphon.SiphonTypeId, InstallationDate = watermetersiphon.RequestSiphon.InstallationDate, InstallationLocation = watermetersiphon.RequestSiphon.InstallationLocation, SiphonDiameterId = watermetersiphon.RequestSiphon.SiphonDiameterId, SiphonMaterialId = watermetersiphon.RequestSiphon.SiphonMaterialId, WaterMeterId = waterMeter.Id, Id = watermetersiphon.RequestSiphon.Id }).ToList();
        ICollection<RequestIndividualEstate> individualEstates = await _individualEstateQueryService.GetByEstateId(estate.Id);
        ICollection<RequestIndividualGetDto> individualsGetDto = individualEstates.Select(individualestate =>
        {
            return new RequestIndividualGetDto()
            {
                Id = individualestate.RequestIndividual.Id,
                FirstName = individualestate.RequestIndividual.FirstName,
                Surname = individualestate.RequestIndividual.Surname,
                NationalId = individualestate.RequestIndividual.NationalId,
                FatherName = individualestate.RequestIndividual.FatherName,
                PhoneNumbers = individualestate.RequestIndividual.PhoneNumbers,
                MobileNumbers = individualestate.RequestIndividual.MobileNumbers,
                IndividualTypeId = individualestate.RequestIndividual.IndividualTypeId,
                IndividualEstateRelationTypeId = individualestate.IndividualEstateRelationTypeId,
            };
        }).ToList();
        foreach (var individual in individualsGetDto)
        {
            individual.Tags = await GetGetIndividualTags(individual.Id);
            individual.IndividualDiscountTypes = await GetGetIndividualDiscountTypes(individual.Id);
        }

        RequestSubscriptionGetDto requestSubscriptionGetDto = new RequestSubscriptionGetDto();
        requestSubscriptionGetDto.WaterMeter = waterMeterGetDto;
        requestSubscriptionGetDto.Estate = estatesGetDto;
        requestSubscriptionGetDto.Individuals = individualsGetDto;
        requestSubscriptionGetDto.Siphons = siphonsGetDto;
        requestSubscriptionGetDto.Flats = flastsGetDto;
        return requestSubscriptionGetDto;
    }

    private async Task<ICollection<RequestIndividualTagGetDto>> GetGetIndividualTags(int individualId)
    {
        ICollection<RequestIndividualTag> individualtags = await _individualTagQueryService.GetByIndividualId(individualId);
        return _mapper.Map<ICollection<RequestIndividualTagGetDto>>(individualtags);
    }

    private async Task<ICollection<RequestIndividualDiscountTypeGetDto>> GetGetIndividualDiscountTypes(int individualId)
    {
        ICollection<RequestIndividualDiscountType> IndividualDiscountTypes = await _individualDiscountTypeQueryService.GetByIndividualId(individualId);
        return _mapper.Map<ICollection<RequestIndividualDiscountTypeGetDto>>(IndividualDiscountTypes);
    }
}
