using Aban360.Common.BaseEntities;
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
        { 
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>> GetInfo(NonPermanentBranchInputDto input)
        {
            string nonPremanentBranchQuery = GetNonPermanentBranchQuery();
            var @params = new
            {
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,

                zoneIds = input.ZoneIds
            };

            IEnumerable<NonPermanentBranchDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchDataOutputDto>(nonPremanentBranchQuery, @params);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber= input.FromReadingNumber,
                ToReadingNumber= input.ToReadingNumber,
                RecordCount = (nonPremanentBranchData is not null && nonPremanentBranchData.Any()) ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>(ReportLiterals.NonPermanentBranchDetail, nonPremanentBranchHeader, nonPremanentBranchData);

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
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        c.ZoneId in @ZoneIds AND
						c.IsNonPermanent=1";
        }
    }
}
