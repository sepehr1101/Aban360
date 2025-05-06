using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UserLeaveUpdateHandler : IUserLeaveUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUserLeaveQueryService _userLeaveQueryService;
        private readonly IValidator<UserLeaveUpdateDto> _validator;

        public UserLeaveUpdateHandler(
            IMapper mapper,
            IUserLeaveQueryService userLeaveQueryService,
            IValidator<UserLeaveUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _userLeaveQueryService = userLeaveQueryService;
            _userLeaveQueryService.NotNull(nameof(_userLeaveQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(UserLeaveUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var userLeave = await _userLeaveQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, userLeave);
        }
    }
}
