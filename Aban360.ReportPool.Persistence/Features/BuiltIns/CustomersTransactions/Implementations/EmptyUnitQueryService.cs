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
    internal sealed class EmptyUnitQueryService : AbstractBaseConnection, IEmptyUnitQueryService
    {
        public EmptyUnitQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }
        public async Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> GetInfo(EmptyUnitInputDto input)
        {
            string emptyUnitQuery = GetEmptyUnitQuery();
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.FromEmptyUnit,
                input.ToEmptyUnit,

                UsageIds = input.UsageSellIds,
                input.ZoneIds
            };

            IEnumerable<EmptyUnitDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitDataOutputDto>(emptyUnitQuery, @params);
            EmptyUnitHeaderOutputDto emptyUnitHeader = new EmptyUnitHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumDomesticCount = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.CommercialUnit) : 0,
                SumOtherCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.TotalUnit) : 0,
                SumEmptyUnit = (emptyUnitData is not null && emptyUnitData.Any()) ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,
            };


            var result = new ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>(ReportLiterals.EmptyUnit, emptyUnitHeader, emptyUnitData);

            return result;
        }
        private string GetEmptyUnitQuery()
        {
            return @"SELECT 
						t46.C2 AS RegionTitle,
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        TRIM(c.Address) AS Address,
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
                        (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalUnit,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.ContractCapacity AS ContractualCapacity,
            	        TRIM(c.BillId) BillId,
            			c.EmptyCount As EmptyUnit,
                        c.ZoneId,
						c.ZoneTitle,
                        t46.C0 AS RegionId,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.PostalCode) AS PostalCode , 
						TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.FatherName)AS FatherName
                    FROM [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    WHERE 
            			c.ToDayJalali IS NULL AND
            			c.UsageId in @UsageIds AND
                        (@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND 
                        c.ZoneId in @ZoneIds AND
            			c.EmptyCount BETWEEN @FromEmptyUnit AND @ToEmptyUnit";
        }
    }
}
