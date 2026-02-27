using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Implementations
{
    internal sealed class BillInstallmentGetHandler : IBillInstallmentGetHandler
    {
        private readonly IGhestAbQueryService _ghestAbQueryService;
        private readonly ICustomerInfoDetailQueryService _customerInfoService;
        private readonly IMembersQueryService _membersQueryService;

        public BillInstallmentGetHandler(
            IGhestAbQueryService ghestAbQueryService,
            ICustomerInfoDetailQueryService customerInfoService,
            IMembersQueryService membersQueryService)
        {
            _ghestAbQueryService = ghestAbQueryService;
            _ghestAbQueryService.NotNull(nameof(ghestAbQueryService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(customerInfoService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));
        }

        public async Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto>> Handle(string input, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdCustomerNumber = await _customerInfoService.GetZoneIdCustomerNumber(input);
            IEnumerable<BillInstallmentOutputDto> data = await _ghestAbQueryService.Get(zoneIdCustomerNumber);
            foreach (var dataItem in data)
            {
                dataItem.BillId = input;
                dataItem.PaymentId = TransactionIdGenerator.GeneratePaymentId(dataItem.Payable, input, $"00{dataItem.QueueNumber}");
            }
            MemberGetDto memberInfo = await _membersQueryService.Get(input);

            return GetResult(data, memberInfo);
        }
        private ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto> GetResult(IEnumerable<BillInstallmentOutputDto> data, MemberGetDto memberInfo)
        {
            BillInstallmentHeaderOutputDto header = new()
            {
                FullName = memberInfo.FullName,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageTitle = memberInfo.UsageTitle,
                Payable = memberInfo.LatestDebt,
                BillId = memberInfo.BillId,
                MobileNumber = memberInfo.MobileNumber,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber
            };
            data.ForEach(b => b.QueueNumberTitle = $"قسط {b.QueueNumber.NumberToText(Language.Persian)}");

            return new ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentOutputDto>("اقساط آب‌بها", header, data);
        }
    }
}
