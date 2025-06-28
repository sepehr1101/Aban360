using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchQueryService : AbstractBaseConnection, INonPermanentBranchQueryService
    {
        public NonPermanentBranchQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string nonPremanentBranchQuery = GetNonPermanentBranchQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds
            };

            IEnumerable<NonPermanentBranchDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchDataOutputDto>(nonPremanentBranchQuery, @params);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = nonPremanentBranchData.Count(),
                ReportDate = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>(ReportLiterals.NonPermanentBranch, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }

        private string GetNonPermanentBranchQuery()
        {
            return @"SELECT 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        0 AS DebtAmount,
                        TRIM(c.Address) AS Address,
                        c.ZoneTitle,
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
            	        TRIM(c.BillId) BillId
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
                        c.RegisterDayJalali BETWEEN @FromDate AND @ToDate AND
                        c.ZoneId in @ZoneIds AND
						c.IsNonPermanent=1";//Todo: check IsNonPermanent Prop
        }
    }
}
