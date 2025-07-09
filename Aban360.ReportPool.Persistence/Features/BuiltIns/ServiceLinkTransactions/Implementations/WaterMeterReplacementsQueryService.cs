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

            var @params = new
            {
                fromDate=input.FromDateJalali,
                toDate=input.ToDateJalali,
                zoneIds=input.ZoneIds,
                isChangeDate=input.IsChangeDate?1:0,
            };
            IEnumerable<WaterMeterReplacementsDataOutputDto> waterMeterReplacementsData = await _sqlReportConnection.QueryAsync<WaterMeterReplacementsDataOutputDto>(waterMeterReplacementss,@params);
            WaterMeterReplacementsHeaderOutputDto waterMeterReplacementsHeader = new WaterMeterReplacementsHeaderOutputDto()
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                ReportDate = DateTime.Now.ToShortPersianDateString(),
                RecordCount=waterMeterReplacementsData.Count()
            };

            var result = new ReportOutput<WaterMeterReplacementsHeaderOutputDto, WaterMeterReplacementsDataOutputDto>(
                input.IsChangeDate==true?  ReportLiterals.WaterMeterReplacements(ReportLiterals.ChangeDate):
                ReportLiterals.WaterMeterReplacements(ReportLiterals.RegisterDate),
                waterMeterReplacementsHeader, 
                waterMeterReplacementsData);
            
            return result;
        }

        private string GetWaterMeterReplacementsQuery()
        {
            return @"Select 
                    	mc.CustomerNumber,
                    	c.ReadingNumber,
                    	c.FirstName +' '+c.SureName AS FullName,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	mc.ChangeDateJalali AS MeterChangeDate,
                    	mc.RegisterDateJalali AS RegistrationDate,
                    	c.MeterSerialBody AS MeterSerial,
                    	c.ZoneTitle AS ZoneTitle
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Clients c on mc.CustomerNumber=c.CustomerNumber AND mc.ZoneId=c.ZoneId
                    Where 
                    	(@isChangeDate=0 AND
                    	mc.RegisterDateJalali BETWEEN @fromDate AND @toDate AND
                    	mc.ZoneId IN @zoneIds)
                    	OR
                    	(@isChangeDate=1 AND
                    	mc.ChangeDateJalali BETWEEN @fromDate AND @toDate AND
                    	mc.ZoneId IN @zoneIds)
                    Order By
                    	mc.RegisterDateJalali Desc,
                    	c.RegisterDayJalali Desc";
        }
    }
}
