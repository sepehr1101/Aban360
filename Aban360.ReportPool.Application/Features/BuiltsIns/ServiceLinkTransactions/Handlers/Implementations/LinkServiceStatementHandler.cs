using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Implementations
{
    internal sealed class LinkServiceStatementHandler : ILinkServiceStatementHandler
    {
        private readonly ILinkServiceStatementQueryService _linkServiceStatementQueryService;
        private readonly IValidator<LinkServiceStatementInputDto> _validator;
        public LinkServiceStatementHandler(
            ILinkServiceStatementQueryService linkServiceStatementQueryService,
            IValidator<LinkServiceStatementInputDto> validator)
        {
            _linkServiceStatementQueryService = linkServiceStatementQueryService;
            _linkServiceStatementQueryService.NotNull(nameof(linkServiceStatementQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto>> Handle(LinkServiceStatementInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ReportOutput<LinkServiceStatementHeaderOutputDto, LinkServiceStatementDataOutputDto> linkServiceStatement = await _linkServiceStatementQueryService.GetInfo(input);
            return linkServiceStatement;
        }
    }
}
