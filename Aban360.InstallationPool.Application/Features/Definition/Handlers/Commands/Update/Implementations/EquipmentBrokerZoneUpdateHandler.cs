using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Features.Queries.Contracts;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Implementations
{
    internal sealed class EquipmentBrokerZoneUpdateHandler : IEquipmentBrokerZoneUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEquipmentBrokerZoneQueryService _equipmentBrokerZoneQueryService;
        private readonly IZoneTitleAddhoc _zoneTitleAddHock;
        private readonly IValidator<EquipmentBrokerZoneUpdateDto> _validator;
        public EquipmentBrokerZoneUpdateHandler(
            IMapper mapper,
            IEquipmentBrokerZoneQueryService equipmentBrokerZoneQueryService,
            IZoneTitleAddhoc zoneTitleAddHhock,
            IValidator<EquipmentBrokerZoneUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _equipmentBrokerZoneQueryService = equipmentBrokerZoneQueryService;
            _equipmentBrokerZoneQueryService.NotNull(nameof(_equipmentBrokerZoneQueryService));

            _zoneTitleAddHock = zoneTitleAddHhock;
            _zoneTitleAddHock.NotNull(nameof(_zoneTitleAddHock));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var equipmentBrokerZone = await _equipmentBrokerZoneQueryService.Get(updateDto.Id);
            equipmentBrokerZone.ZoneTitle = await _zoneTitleAddHock.Handle(updateDto.ZoneId, cancellationToken);
            _mapper.Map(updateDto, equipmentBrokerZone);
        }
    }
}
