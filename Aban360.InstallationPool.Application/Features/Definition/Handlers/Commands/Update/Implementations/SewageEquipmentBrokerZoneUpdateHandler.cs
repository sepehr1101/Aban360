using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneUpdateHandler : ISewageEquipmentBrokerZoneUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerZoneQueryService _sewageEquipmentBrokerZoneQueryService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        private readonly IValidator<SewageEquipmentBrokerZoneUpdateDto> _validator;
        public SewageEquipmentBrokerZoneUpdateHandler(
            IMapper mapper,
            ISewageEquipmentBrokerZoneQueryService sewageEquipmentBrokerZoneQueryService,
            IZoneTitleAddhoc zoneTitleAddHhock,
            IValidator<SewageEquipmentBrokerZoneUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerZoneQueryService = sewageEquipmentBrokerZoneQueryService;
            _sewageEquipmentBrokerZoneQueryService.NotNull(nameof(_sewageEquipmentBrokerZoneQueryService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SewageEquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var sewageEquipmentBrokerZone = await _sewageEquipmentBrokerZoneQueryService.Get(updateDto.Id);
            sewageEquipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(updateDto.ZoneId, cancellationToken);
            _mapper.Map(updateDto, sewageEquipmentBrokerZone);
        }
    }
}
