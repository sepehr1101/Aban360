using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class FlatUpdateHandler : IFlatUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IFlatQueryService _queryService;
        private readonly IValidator<FlatUpdateDto> _flatValidator;
        public FlatUpdateHandler(
            IMapper mapper,
            IFlatQueryService queryService,
            IValidator<FlatUpdateDto> estateValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _flatValidator = estateValidator;
            _flatValidator.NotNull(nameof(estateValidator));
        }

        public async Task Handle(FlatUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _flatValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }


            Flat flat = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, flat);
        }
    }
}
