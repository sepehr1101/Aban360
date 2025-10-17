using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterResourceCreateHandler : IWaterResourceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterResourceCommandService _waterResourceCommandService;
        private readonly IHeadquartersAddhoc _headquartersAddhoc;
        private readonly IValidator<WaterResourceCreateDto> _validator;

        public WaterResourceCreateHandler(
            IMapper mapper,
            IWaterResourceCommandService WaterResourceCommandService,
            IHeadquartersAddhoc headquartersAddhoc,
            IValidator<WaterResourceCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterResourceCommandService = WaterResourceCommandService;
            _waterResourceCommandService.NotNull(nameof(_waterResourceCommandService));

            _headquartersAddhoc = headquartersAddhoc;
            _headquartersAddhoc.NotNull(nameof(_headquartersAddhoc));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            WaterResource waterResource = _mapper.Map<WaterResource>(createDto);
            if (waterResource == null)
            {
                throw new InvalidDataException();
            }
            string headquartersTitle = await _headquartersAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
            waterResource.HeadquartersTitle = headquartersTitle;
            await _waterResourceCommandService.Add(waterResource);
        }
    }
}
