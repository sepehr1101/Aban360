using Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Queries.Implementations
{
    internal sealed class RequestSubscriptionGetHandler : IRequestSubscriptionGetHandler
    {
        private readonly IRequestWaterMeterQueryService _waterMeterQueryService;
        private readonly IRequestEstateQueryService _estateQueryService;
        private readonly IRequestFlatQueryService _flatQueryService;
        private readonly IRequestWaterMeterSiphonQueryService _waterMeterSiphonQueryService;
        private readonly IRequestIndividualEstateQueryService _individualEstateQueryService;
        private readonly IRequestIndividualTagQueryService _individualTagQueryService;
        private readonly IRequestIndividualDiscountTypeQueryService _individualDiscountTypeQueryService;
        private readonly IMapper _mapper;
        public RequestSubscriptionGetHandler(
            IRequestWaterMeterQueryService waterMeterQueryService,
            IRequestEstateQueryService estateQueryService,
            IRequestFlatQueryService flatQueryService,
            IRequestWaterMeterSiphonQueryService waterMeterSiphonQueryService,
            IRequestIndividualEstateQueryService individualEstateQueryService,
            IRequestIndividualTagQueryService individualTagQueryService,
            IRequestIndividualDiscountTypeQueryService individualDiscountTypeQueryService,
            IMapper mapper)
        {
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

            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));
        }
        public async Task<RequestSubscriptionGetDto> Handle(string billId, CancellationToken cancellationToken)
        {
            RequestWaterMeter waterMeter = await _waterMeterQueryService.Get(billId);
            RequestWaterMeterGetDto waterMeterGetDto = _mapper.Map<RequestWaterMeterGetDto>(waterMeter);//todo: mapper

            RequestEstate estate = await _estateQueryService.Get(waterMeter.EstateId);
            RequestEstateGetDto estatesGetDto = _mapper.Map<RequestEstateGetDto>(estate);

            ICollection<RequestFlat> flats = await _flatQueryService.GetByEstateId(waterMeter.EstateId);
            ICollection<RequestFlatGetDto> flastsGetDto = _mapper.Map<ICollection<RequestFlatGetDto>>(flats);

            ICollection<RequestWaterMeterSiphon> waterMeterSiphons = await _waterMeterSiphonQueryService.GetByWaterMeterId(waterMeter.Id);
            ICollection<RequestSiphonGetDto> siphonsGetDto = waterMeterSiphons
                .Select(watermetersiphon => new RequestSiphonGetDto()
                {
                    SiphonTypeId = watermetersiphon.RequestSiphon.SiphonTypeId,
                    InstallationDate = watermetersiphon.RequestSiphon.InstallationDate,
                    InstallationLocation = watermetersiphon.RequestSiphon.InstallationLocation,
                    SiphonDiameterId = watermetersiphon.RequestSiphon.SiphonDiameterId,
                    SiphonMaterialId = watermetersiphon.RequestSiphon.SiphonMaterialId,
                    WaterMeterId = waterMeter.Id,
                    Id = watermetersiphon.RequestSiphon.Id
                })
                .ToList();

            ICollection<RequestIndividualEstate> individualEstates = await _individualEstateQueryService.GetByEstateId(estate.Id);
            ICollection<RequestIndividualGetDto> individualsGetDto = individualEstates
                .Select(individualestate =>
                {
                    return new RequestIndividualGetDto()
                    {
                        Id = individualestate.RequestIndividual.Id,
                        FullName = individualestate.RequestIndividual.FullName,
                        NationalId = individualestate.RequestIndividual.NationalId,
                        FatherName = individualestate.RequestIndividual.FatherName,
                        PhoneNumbers = individualestate.RequestIndividual.PhoneNumbers,
                        MobileNumbers = individualestate.RequestIndividual.MobileNumbers,
                        IndividualTypeId = individualestate.RequestIndividual.IndividualTypeId,
                        IndividualEstateRelationTypeId = individualestate.IndividualEstateRelationTypeId,
                    };
                })
                .ToList();
            foreach (var individual in individualsGetDto)
            {
                individual.Tags = await GetIndividualTags(individual.Id);
                individual.IndividualDiscountTypes = await GetIndividualDiscountTypes(individual.Id);
            }

            RequestSubscriptionGetDto requestSubscriptionGetDto = new RequestSubscriptionGetDto();
            requestSubscriptionGetDto.WaterMeter = waterMeterGetDto;
            requestSubscriptionGetDto.Estate = estatesGetDto;
            requestSubscriptionGetDto.Individuals = individualsGetDto;
            requestSubscriptionGetDto.Siphons = siphonsGetDto;
            requestSubscriptionGetDto.Flats = flastsGetDto;

            return requestSubscriptionGetDto;
        }
        private async Task<ICollection<RequestIndividualTagGetDto>> GetIndividualTags(int individualId)
        {
            ICollection<RequestIndividualTag> individualtags = await _individualTagQueryService.GetByIndividualId(individualId);
            return _mapper.Map<ICollection<RequestIndividualTagGetDto>>(individualtags);

        }

        private async Task<ICollection<RequestIndividualDiscountTypeGetDto>> GetIndividualDiscountTypes(int individualId)
        {
            ICollection<RequestIndividualDiscountType> IndividualDiscountTypes = await _individualDiscountTypeQueryService.GetByIndividualId(individualId);
            return _mapper.Map<ICollection<RequestIndividualDiscountTypeGetDto>>(IndividualDiscountTypes);
        }
    }

}