using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetReportByTileScriptContentHandler : IGetReportByTileScriptContentHandler
    {
        private readonly ITileScriptService _tileScriptService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<TileScriptInputDto> _validator;

        public GetReportByTileScriptContentHandler(
            ITileScriptService service,
            ICommonZoneService commonZoneService,
            IValidator<TileScriptInputDto> validator)
        {
            _tileScriptService = service;
            _tileScriptService.NotNull(nameof(_tileScriptService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(_commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }
         public async Task<IEnumerable<TileScriptReportDto>> Handle(int id, IAppUser appUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            TileScript tileScript = await _tileScriptService.GetById(id);
            if (tileScript != null && tileScript.Content != null)
            {
                IEnumerable<int> zoneIds = await _commonZoneService.GetMyZoneIds(appUser);
                TileScriptContentReportInputDto tileScriptContentInput = new(zoneIds, DateTime.Now.ToShortPersianDateString(), null, null);
                IEnumerable<TileScriptReportDto> report = await _tileScriptService.GetContent(tileScript.Content, tileScriptContentInput);
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