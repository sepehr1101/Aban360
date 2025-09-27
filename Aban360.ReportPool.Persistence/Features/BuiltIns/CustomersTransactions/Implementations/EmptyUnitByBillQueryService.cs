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
    internal sealed class EmptyUnitByBillQueryService : AbstractBaseConnection, IEmptyUnitByBillQueryService
    {
        public EmptyUnitByBillQueryService(IConfiguration configuration)
            : base(configuration)
        {
		}
        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> GetInfo(EmptyUnitInputDto input)
        {
            string emptyUnitQuery = GetEmptyUnitQuery(input.ZoneIds.Any(), input.UsageSellIds.Any());
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromUnit = input.FromEmptyUnit,
                toUnit = input.ToEmptyUnit,

                UsageIds = input.UsageSellIds,
                zoneIds = input.ZoneIds
            };

            IEnumerable<EmptyUnitDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitDataOutputDto>(emptyUnitQuery, @params);
            EmptyUnitHeaderOutputDto emptyUnitHeader = new EmptyUnitHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                CustomerCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                RecordCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.TotalUnit) : 0,
                SumEmptyUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,

            };

            var result = new ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>(ReportLiterals.EmptyUnitByBillDetail, emptyUnitHeader, emptyUnitData);
            return result;
        }

        private string GetEmptyUnitQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId in @usageIds" : string.Empty;
            return @$";WITH EmptyUnitByBill AS
					(
					    SELECT
							b.CustomerNumber,
							b.ReadingNumber,
							TRIM(c.FirstName) AS FirstName,
							TRIM(c.SureName) As Surname,
							b.UsageTitle,
							b.WaterDiameterTitle MeterDiameterTitle,
							c.RegisterDayJalali AS EventDateJalali,
							TRIM(c.Address) AS Address,
							c.DeletionStateId,
							c.DeletionStateTitle AS UseStateTitle,
							b.DomesticCount DomesticUnit,
							b.CommercialCount CommercialUnit,
							b.OtherCount OtherUnit,
                            (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalUnit,
                            c.MainSiphonTitle AS SiphonDiameterTitle,
                            c.ContractCapacity AS ContractualCapacity,
							TRIM(c.BillId) BillId,
							b.EmptyCount As EmptyUnit,
							b.ZoneId,
							b.ZoneTitle,
							c.NationalId AS NationalCode,
							c.PostalCode , 
							c.PhoneNo AS PhoneNumber,
						    TRIM(c.MobileNo) AS MobileNumber,
							c.FatherName ,
							b.Consumption,
							b.ConsumptionAverage,
							b.SumItems,
					        ROW_NUMBER() OVER (
					            PARTITION BY b.BillId
					            ORDER BY b.Id DESC
					        ) AS RowNum
					    FROM [CustomerWarehouse].dbo.Bills b
						Join [CustomerWarehouse].dbo.Clients c On b.CustomerNumber=c.CustomerNumber and b.ZoneId=c.ZoneId
					    WHERE
							(b.EmptyCount BETWEEN @fromUnit AND @toUnit)
							AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                            c.ToDayJalali is null
							{zoneQuery}	
							{usageQuery}
					)
					SELECT 
						    e.CustomerNumber,
							e.ReadingNumber,
							e.FirstName,
							e.Surname,
							e.UsageTitle,
							e.MeterDiameterTitle,
							e.EventDateJalali,
							e.Address,
							e.OtherUnit,
							e.DeletionStateId,
							e.UseStateTitle,
							e.DomesticUnit,
							e.CommercialUnit,
							e.BillId,
							e.EmptyUnit,
							e.ZoneId,
							e.ZoneTitle,							
							t46.C2 AS RegionTitle,
							t46.C0 AS RegionId,
							e.NationalCode,
							e.PostalCode , 
							e.PhoneNumber,
						    e.MobileNumber,
							e.FatherName ,
							e.Consumption,
							e.ConsumptionAverage,
							e.SumItems,
							e.ContractualCapacity ,
							e.SiphonDiameterTitle
					FROM EmptyUnitByBill e
					Join [Db70].dbo.T51 t51
						On t51.C0=e.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					WHERE RowNum = 1;";
        }
    }
}
