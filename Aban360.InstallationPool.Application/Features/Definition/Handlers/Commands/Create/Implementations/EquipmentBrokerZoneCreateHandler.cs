using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Entities;
using Aban360.InstallationPool.Persistence.Features.Commands.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Implementations
{
    internal sealed class EquipmentBrokerZoneCreateHandler : IEquipmentBrokerZoneCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerZoneCommandService _equipmentBrokerZoneCommandService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        private readonly IValidator<EquipmentBrokerZoneCreateDto> _validator;
        public EquipmentBrokerZoneCreateHandler(
            IMapper mapper,
            IEquipmentBrokerZoneCommandService equipmentBrokerZoneCommandService,
            IZoneTitleAddhoc zoneTitleAddHhock,
            IValidator<EquipmentBrokerZoneCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerZoneCommandService = equipmentBrokerZoneCommandService;
            _equipmentBrokerZoneCommandService.NotNull(nameof(_equipmentBrokerZoneCommandService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var equipmentBrokerZone = _mapper.Map<EquipmentBrokerZone>(createDto);
            equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(createDto.ZoneId,cancellationToken);
            await _equipmentBrokerZoneCommandService.Add(equipmentBrokerZone);
        }
    }
}
