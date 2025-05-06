using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class EstateWaterResourceCreateHandler : IEstateWaterResourceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceCommandService _estateWaterResourceCommandService;
        private readonly IValidator<EstateWaterResourceCreateDto> _validator;

        public EstateWaterResourceCreateHandler(
            IMapper mapper,
            IEstateWaterResourceCommandService EstateWaterResourceCommandService,
            IValidator<EstateWaterResourceCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceCommandService = EstateWaterResourceCommandService;
            _estateWaterResourceCommandService.NotNull(nameof(_estateWaterResourceCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            EstateWaterResource estateWaterResource = _mapper.Map<EstateWaterResource>(createDto);
            if (estateWaterResource == null)
            {
                throw new InvalidDataException();
            }
            await _estateWaterResourceCommandService.Add(estateWaterResource);
        }
    }
}
