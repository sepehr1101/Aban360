using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WaterMeterReplacementsQueryService : AbstractBaseConnection, IWaterMeterReplacementsQueryService
    {
        public WaterMeterReplacementsQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>> GetInfo(WaterMeterReplacementsInputDto input)
        {
            string waterMeterReplacementss = GetWaterMeterReplacementsQuery();
            string reportTitle = input.IsChangeDate == true ? ReportLiterals.WaterMeterReplacements(ReportLiterals.ChangeDate) :
                ReportLiterals.WaterMeterReplacements(ReportLiterals.RegisterDate);

            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,
                usageIds = input.UsageIds,
                isChangeDate = input.IsChangeDate ? 1 : 0,
            };
            IEnumerable<WaterMeterReplacementsDataOutputDto> waterMeterReplacementsData = await _sqlReportConnection.QueryAsync<WaterMeterReplacementsDataOutputDto>(waterMeterReplacementss, @params);
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterMeterReplacementsData is not null && waterMeterReplacementsData.Any()) ? waterMeterReplacementsData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = waterMeterReplacementsData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = waterMeterReplacementsData.Sum(i => i.DomesticUnit),
                SumOtherUnit = waterMeterReplacementsData.Sum(i => i.OtherUnit),
                TotalUnit = waterMeterReplacementsData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>(
                reportTitle,
                waterMeterReplacementsHeader,
                waterMeterReplacementsData);

            return result;
        }

        private string GetWaterMeterReplacementsQuery()
        {
            return @"Select 
                    	mc.CustomerNumber,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) AS FullName,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	mc.ChangeDateJalali AS MeterChangeDate,
                    	mc.RegisterDateJalali AS RegistrationDate,
                    	c.MeterSerialBody AS MeterSerial,
                    	c.ZoneTitle AS ZoneTitle,
                        mc.ChangeCauseTitle,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.ContractCapacity AS ContractualCapacity
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Clients c 
                        on mc.CustomerNumber=c.CustomerNumber AND mc.ZoneId=c.ZoneId
                    Where 
                    	(@isChangeDate=0 AND
                    	mc.RegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL )
                    	OR
                    	(@isChangeDate=1 AND
                    	mc.ChangeDateJalali BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
						(@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL )
                    Order By
                    	mc.RegisterDateJalali Desc,
                    	c.RegisterDayJalali Desc";
        }
    }
}
