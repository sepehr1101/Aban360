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
    internal sealed class EstateBoundTypeUpdateHandler : IEstateBoundTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateBoundTypeQueryService _queryService;
        private readonly IValidator<EstateBoundTypeUpdateDto> _validator;

        public EstateBoundTypeUpdateHandler(
            IMapper mapper,
            IEstateBoundTypeQueryService queryService,
            IValidator<EstateBoundTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(EstateBoundTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            EstateBoundType estateBoundType = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, estateBoundType);
        }
    }
}
