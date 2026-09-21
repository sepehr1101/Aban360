using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class InvalidPaymentIdGetHandler : IInvalidPaymentIdGetHandler
    {
        private readonly IBedBesQueryService _bedBesQueryService;
        private static string _title = ReportLiterals.InvalidPaymentId;
        public InvalidPaymentIdGetHandler(IBedBesQueryService bedBesQueryService)
        {
            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));
        }

        public async Task<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>> Handle(InvalidPaymentIdInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<InvalidPaymentIdDataOutputDto> data = await _bedBesQueryService.Get(input);
            InvalidPaymentIdHeaderOutputDto header = new()
            {
                ZoneId = input.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                BillCount = data?.Count() ?? 0,
                CustomerCount = data?.DistinctBy(d => d.CustomerNumber)?.Count() ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _title
            };
            ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto> result = new(_title, header, data);
            return result;
        }
    }
}
