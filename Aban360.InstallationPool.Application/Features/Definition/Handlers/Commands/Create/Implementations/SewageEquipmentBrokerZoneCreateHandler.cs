using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class SewageEquipmentBrokerZoneCreateHandler : ISewageEquipmentBrokerZoneCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISewageEquipmentBrokerZoneCommandService _sewageEquipmentBrokerZoneCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        private readonly IValidator<SewageEquipmentBrokerZoneCreateDto> _validator;
        public SewageEquipmentBrokerZoneCreateHandler(
            IMapper mapper,
                ISewageEquipmentBrokerZoneCommandService sewageEquipmentBrokerZoneCommandService,
            IZoneTitleAddhoc zoneTitleAddHhock,
            IValidator<SewageEquipmentBrokerZoneCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _sewageEquipmentBrokerZoneCommandService = sewageEquipmentBrokerZoneCommandService;
            _sewageEquipmentBrokerZoneCommandService.NotNull(nameof(_sewageEquipmentBrokerZoneCommandService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(SewageEquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var sewageEquipmentBrokerZone = _mapper.Map<SewageEquipmentBrokerZone>(createDto);
            sewageEquipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(createDto.ZoneId,cancellationToken);
            await _sewageEquipmentBrokerZoneCommandService.Add(sewageEquipmentBrokerZone);
        }
    }
}
