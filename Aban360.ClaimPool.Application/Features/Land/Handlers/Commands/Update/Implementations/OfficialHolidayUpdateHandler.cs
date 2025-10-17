using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class OfficialHolidayUpdateHandler : IOfficialHolidayUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        private readonly IValidator<OfficialHolidayUpdateDto> _validator;

        public OfficialHolidayUpdateHandler(
            IMapper mapper,
            IOfficialHolidayQueryService officialHolidayQueryService,
            IValidator<OfficialHolidayUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(_officialHolidayQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(OfficialHolidayUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var officialHoliday = await _officialHolidayQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, officialHoliday);
        }
    }
}
