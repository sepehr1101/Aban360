using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class BasicInfoChangeHistoryQueryService : ChangeHistoryBase, IBasicInfoChangeHistoryQueryService
    {
        public BasicInfoChangeHistoryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto>> GetInfo(BasicInfoChangeHistoryInputDto input)
        {
            var (changeTitle, changeField) = GetItemChangeProperty(input.ItemChange);
            string title = $"{ReportLiterals.BasicInfoChangeHistory} - {changeTitle}";
            string query = GetQuery(changeField);

            IEnumerable<BasicInfoChangeHistoryDataOutputDto> data = await _sqlReportConnection.QueryAsync<BasicInfoChangeHistoryDataOutputDto>(query, input);
            BasicInfoChangeHistoryHeaderOutputDto header = new BasicInfoChangeHistoryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                CustomerCount = data is not null && data.Any() ? data.Count() : 0,
                RecordCount = data is not null && data.Any() ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = title,
            };


            var result = new ReportOutput<BasicInfoChangeHistoryHeaderOutputDto, BasicInfoChangeHistoryDataOutputDto>(title, header, data);

            return result;
        }
        private string GetQuery(string titleField)
        {
            return $@";WITH History AS
                    (
                        SELECT
                            Id,
                            BillId,
                            RegisterDayJalali,
                    		{titleField} ItemTitle,
                            LAG({titleField}) OVER(PARTITION BY BillId ORDER BY RegisterDayJalali, Id) AS PreviousItemTitle
                        FROM CustomerWarehouse.dbo.Clients
                    	Where 
                            ZoneId IN @ZoneIds AND
                            (RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali) AND
                            (
                                 @FromReadingNumber IS NULL OR
                                 @ToReadingNumber IS NULL OR
                                 ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber
                            ) 
                    )
                    SELECT
	                    t46.C0 RegionId,
						t46.C2 RegionTitle,
						c.ZoneId,
						c.ZoneTitle,
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	c.BillId,
                    	c.FirstName,
                    	c.SureName Surname,
                    	c.FirstName + ' ' + c.SureName FullName,
                    	c.UsageId,
                    	c.UsageTitle,
                    
                        h.RegisterDayJalali AS ChangeDateJalali, 
                    	h.ItemTitle FromItem,
                        h.PreviousItemTitle ToItem
                    FROM History h
                    JOIN CustomerWarehouse.dbo.Clients c
                        ON c.BillId = h.BillId
					JOIN Db70.dbo.T51 t51
						ON t51.C0=c.ZoneId
					JOIN Db70.dbo.T46 t46
						ON t46.C0=t51.C1
                    WHERE
                    	c.ToDayJalali IS NULL AND
                        c.ZoneId IN @ZoneIds AND     
                         (
                            @FromReadingNumber IS NULL OR
                            @ToReadingNumber IS NULL OR
                            c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber
                         ) AND 
                        h.PreviousItemTitle IS NOT NULL AND
                        h.PreviousItemTitle <> h.ItemTitle
                    ORDER BY
                        h.BillId,
                        h.RegisterDayJalali,
                        h.Id;";
        }
        private (string, string) GetItemChangeProperty(CustomerBasicPropertyEnum inputType)
        {
            return inputType switch
            {
                CustomerBasicPropertyEnum.Usage => ("کاربری", "UsageTitle"),
                CustomerBasicPropertyEnum.ContractualCapacity => ("ظرفیت قراردادی", "ContractCapacity"),
                CustomerBasicPropertyEnum.DomesticUnit => ("آحاد مسکونی", "DomesticCount"),
                CustomerBasicPropertyEnum.CommercialUnit => ("آحاد تجاری", "CommercialCount"),
                CustomerBasicPropertyEnum.OtherUnit => ("آحاد سایر", "OtherCount"),
                CustomerBasicPropertyEnum.HouseholdNumber => ("خانواری", "HouseholdDateJalali"),//?????
                CustomerBasicPropertyEnum.EmptyUnit => ("خالی از سکنه", "EmptyCount"),
                CustomerBasicPropertyEnum.BranchType => ("نوع واگذرای", "BranchType"),
                CustomerBasicPropertyEnum.DeletionState => ("وضعیت انشعاب", "DeletionStateTitle"),
                CustomerBasicPropertyEnum.FirstName => ("نام", "FirstName"),
                CustomerBasicPropertyEnum.Surname => ("نام خانوادگی", "SureName"),
                CustomerBasicPropertyEnum.MobileNumber => ("موبایل", "MobileNo"),
                _ => ("کاربری", "UsageTitle")
            };
        }
    }
}
