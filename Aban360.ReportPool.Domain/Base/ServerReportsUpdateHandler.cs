using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts
{
    internal sealed class ServerReportsUpdateHandler : IServerReportsUpdateHandler
    {
        private readonly IServerReportsUpdateService _serverReportsUpdateService;
        private readonly IValidator<ServerReportsUpdateDto> _validator;

        public ServerReportsUpdateHandler(
            IServerReportsUpdateService serverReportsUpdateService,
            IValidator<ServerReportsUpdateDto> validator)
        {
            _serverReportsUpdateService = serverReportsUpdateService;
            _serverReportsUpdateService.NotNull(nameof(serverReportsUpdateService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async void Handle(ServerReportsUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(UpdateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            _serverReportsUpdateService.Update(UpdateDto);
        }
    }
}
