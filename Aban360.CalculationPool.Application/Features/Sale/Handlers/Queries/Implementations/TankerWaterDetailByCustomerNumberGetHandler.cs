using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class TankerWaterDetailByCustomerNumberGetHandler : ITankerWaterDetailByCustomerNumberGetHandler
    {
        private readonly ITankerQueryService _tankerQueryService;
        private readonly IBedBesQueryService _besQueryService;
        const string _title = "آب تانکری";
        public TankerWaterDetailByCustomerNumberGetHandler(
            ITankerQueryService tankerQueryService,
            IBedBesQueryService besQueryService)
        {
            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));

            _besQueryService = besQueryService;
            _besQueryService.NotNull(nameof(besQueryService));
        }

        public async Task<ReportOutput<TankerHeaderOutputDto, StringDictionary>> Handle(ZoneIdAndCustomerNumber input, CancellationToken cancellationToken)
        {
            TankerOutputDto tankerInfo = await _tankerQueryService.Get(input);
            BedBesItemsOutputDto bedBesInfo = await _besQueryService.GetLatestByCustomerNumber(input);
            ICollection<StringDictionary> data = new List<StringDictionary>()
            {
                new StringDictionary("آب‌بها",bedBesInfo.Water.ToString()),
                new StringDictionary("بودجه",bedBesInfo.Budget.ToString()),
                new StringDictionary("مالیات",bedBesInfo.Tax.ToString()),
            };
            TankerHeaderOutputDto header = new()
            {
                ZoneId = tankerInfo.ZoneId,
                ZoneTitle = tankerInfo.ZoneTitle,
                RegionId = tankerInfo.RegionId,
                RegionTitle = tankerInfo.RegionTitle,
                CustomerNumber = tankerInfo.CustomerNumber,
                BillId = bedBesInfo.BillId,
                PayId = bedBesInfo.PayId,
                RegisterDateJalali = tankerInfo.RegisterDateJalali,
                FirstName = tankerInfo.FirstName,
                Surname = tankerInfo.Surname,
                Address = tankerInfo.Address,
                Amount = tankerInfo.Amount,
                Duration = 0,
                Consumption = tankerInfo.Consumption,
                SaleState = string.Empty,
                Title = _title,
            };

            return new ReportOutput<TankerHeaderOutputDto, StringDictionary>(_title, header, data);
        }
    }
}
