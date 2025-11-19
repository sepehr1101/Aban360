using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetReportByTileScriptContentHandler : IGetReportByTileScriptContentHandler
    {
        private readonly ITileScriptService _tileScriptService;
        private readonly IValidator<TileScriptInputDto> _validator;

        public GetReportByTileScriptContentHandler(
            ITileScriptService service,
            IValidator<TileScriptInputDto> validator)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<IEnumerable<TileScriptReportDto>> Handle(int id, string? input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            TileScript tileScript = await _tileScriptService.GetById(id);
            if (tileScript != null && tileScript.Content != null)
            {
                IEnumerable<TileScriptReportDto> report = await _tileScriptService.GetContent(tileScript.Content, input);
                return report;

            }

            return null;
        }
        public async Task<IEnumerable<TileScriptReportDto>> Handle(TileScriptInputDto input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Validation(input, cancellationToken);

            TileScript tileScript = await _tileScriptService.GetById(input.Id);
            if (tileScript != null && tileScript.Content != null)
            {
                IEnumerable<TileScriptReportDto> report = await _tileScriptService.GetContent(tileScript.Content, input.FromDateJalali);
                return report;
            }

            return null;
        }
        private async Task Validation(TileScriptInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}