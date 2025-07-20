using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Implementations
{
    internal sealed class ServerReportsCreateHandler : IServerReportsCreateHandler
    {
        private readonly IServerReportsCreateService _serverReportsCreateService;
        private readonly IValidator<ServerReportsCreateDto> _validator;

        public ServerReportsCreateHandler(
            IServerReportsCreateService serverReportsCreateService,
            IValidator<ServerReportsCreateDto> validator)
        {
            _serverReportsCreateService = serverReportsCreateService;
            _serverReportsCreateService.NotNull(nameof(serverReportsCreateService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async void Handle(ServerReportsCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            _serverReportsCreateService.Create(createDto);
        }
    }
}
