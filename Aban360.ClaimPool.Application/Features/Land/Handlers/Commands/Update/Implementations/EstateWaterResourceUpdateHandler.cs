using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class EstateWaterResourceUpdateHandler : IEstateWaterResourceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateWaterResourceQueryService _estateWaterResourceQueryService;
        private readonly IValidator<EstateWaterResourceUpdateDto> _validator;

        public EstateWaterResourceUpdateHandler(
            IMapper mapper,
            IEstateWaterResourceQueryService EstateWaterResourceQueryService,
            IValidator<EstateWaterResourceUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _estateWaterResourceQueryService = EstateWaterResourceQueryService;
            _estateWaterResourceQueryService.NotNull(nameof(_estateWaterResourceQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            EstateWaterResource estateWaterResource = await _estateWaterResourceQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, estateWaterResource);
        }
    }
}
