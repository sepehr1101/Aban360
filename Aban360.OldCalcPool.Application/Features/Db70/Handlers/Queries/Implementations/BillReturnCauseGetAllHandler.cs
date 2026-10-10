using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using System.Collections.Generic;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class BillReturnCauseGetAllHandler : IBillReturnCauseGetAllHandler
    {
        private readonly IBillReturnCauseQueryService _billReturnCauseQueryService;

        public BillReturnCauseGetAllHandler(IBillReturnCauseQueryService billReturnCauseQueryService)
        {
            _billReturnCauseQueryService = billReturnCauseQueryService;
            _billReturnCauseQueryService.NotNull(nameof(billReturnCauseQueryService));
        }
        public async Task<IEnumerable<BillReturnCauseGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> data = await _billReturnCauseQueryService.Get();
            IEnumerable<BillReturnCauseGetDto> completeResult = data
                .Select(d => new BillReturnCauseGetDto()
                {
                    Id = d.Id,
                    Code = d.Code,
                    Title = $"{d.Title} ({GetTitleCombineReturnType(d.Title, d.IsLastMeterValid)})",
                    IsInList = d.IsInList,
                    IsLastMeterValid = d.IsLastMeterValid,
                    IsPartial = d.IsPartial,
                })
                .ToList();

            return completeResult;
        }
        public async Task<IEnumerable<NumericDictionary>> HandleByDictionary(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _billReturnCauseQueryService.GetByDictionary();
            return result;
        }
        private string GetTitleCombineReturnType(string title, bool isLastMeterValid) => isLastMeterValid ? ReportLiterals.ReturnFull : ReportLiterals.ReturnPartial;
    }
}
