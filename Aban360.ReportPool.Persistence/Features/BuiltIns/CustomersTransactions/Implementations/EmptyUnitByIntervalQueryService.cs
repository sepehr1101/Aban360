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
    internal sealed class EmptyUnitByIntervalQueryService : AbstractBaseConnection, IEmptyUnitByIntervalQueryService
    {
        public EmptyUnitByIntervalQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> GetInfo(EmptyUnitByIntervalInputDto input)
        {
            string query = GetQuery();
            //string query = GetEmptyUnitQuery();

            IEnumerable<EmptyUnitDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitDataOutputDto>(query, input);
            EmptyUnitHeaderOutputDto emptyUnitHeader = new EmptyUnitHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                RecordCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.EmptyUnit,

                SumDomesticUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.CommercialUnit) : 0,
                SumOtherUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.TotalUnit) : 0,
                SumEmptyUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,

            };


            var result = new ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>(ReportLiterals.EmptyUnit, emptyUnitHeader, emptyUnitData);

            return result;
        }

        private string GetQuery()
        {
            return @";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    *
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	    c.ZoneId IN @ZoneIds AND
                    	    c.UsageId IN @UsageSellIds AND
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0
                    )
                    Select	
                    	t46.C2 AS RegionTitle,
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) As Surname,
                    	c.UsageTitle,
                    	c.WaterDiameterTitle MeterDiameterTitle,
                    	c.RegisterDayJalali AS EventDateJalali,
                    	TRIM(c.Address) AS Address,
                    	c.DeletionStateId,
                    	c.DeletionStateTitle AS UseStateTitle,
                    	c.DomesticCount DomesticUnit,
                    	c.CommercialCount CommercialUnit,
                    	c.OtherCount OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	TRIM(c.BillId) BillId,
                    	c.EmptyCount As EmptyUnit,
                    	c.ZoneId,
                    	c.ZoneTitle,
                    	t46.C0 AS RegionId,
                    	TRIM(c.PostalCode) AS PostalCode , 
                    	TRIM(c.NationalId) AS NationalCode,
                    	TRIM(c.PhoneNo) AS PhoneNumber,
                    	TRIM(c.MobileNo) AS MobileNumber,
                    	TRIM(c.FatherName) AS FatherName,
                    	w.Debt SumItems
                     FROM CTE c
                     LEFT JOIN [CustomerWarehouse].dbo.WaterDebt w
                    	 On c.BillId Collate SQL_Latin1_General_CP1_CI_AS= w.BillId
                     JOIN [Db70].dbo.T51 t51
                         On t51.C0=c.ZoneId
                     JOIN [Db70].dbo.T46 t46
                         On t51.C1=t46.C0
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2) AND
                         c.EmptyCount BETWEEN @FromEmptyUnit AND @ToEmptyUnit
					Order By
						t46.C2,
						c.ZoneTitle,
						c.CustomerNumber";
        }
    }
}