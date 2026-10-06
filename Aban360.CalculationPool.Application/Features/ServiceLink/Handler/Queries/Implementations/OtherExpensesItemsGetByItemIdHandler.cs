using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class OtherExpensesItemsGetByItemIdHandler : IOtherExpensesItemsGetByItemIdHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        public OtherExpensesItemsGetByItemIdHandler(
            IOtherExpensesItemsQueryService otherExpensesItemsQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<OtherExpensesItemsDataDto> Handle(string billId, int itemId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdCustomerNumber);

            OtherExpensesItemsDataDto? data = await _otherExpensesItemsQueryService.Get(new OtherExpensesItemsGetDto(memberInfo.ZoneId, memberInfo.UsageId, itemId), false);
            if (data is null)
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundAmount);
            }
            //OtherExpensesItemsDataDto data = new()
            //{
            //    Id = 1,
            //    ServiceId = itemId,
            //    ServiceTitle = itemId.ToString(),
            //    Amount = 10000,
            //    InsertDateTime = DateTime.Now,
            //    RemoveDateTime = null,
            //};
            return data;
        }
    }
}
