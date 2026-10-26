using Aban360.Common.Extensions;
using Aban360.Common.Exceptions;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Services.Implementations
{
    internal static class TicketHandlerValidation
    {
        internal static async Task Validate<T>(IValidator<T> validator, T input, CancellationToken cancellationToken)
        {
            input.NotNull(nameof(input));
            var result = await validator.ValidateAsync(input, cancellationToken);
            if (!result.IsValid)
                throw new CustomValidationException(string.Join(", ", result.Errors.Select(x => x.ErrorMessage)));
        }
    }
}
