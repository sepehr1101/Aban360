using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class BranchSpecificationInfoService : AbstractBaseConnection, IBranchSpecificationInfoService
    {
        public BranchSpecificationInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<BranchSpecificationInfoDto> GetInfo(string billId)
        {
            string BranchSpecificationQuery = GetBranchSpecificationSummayDtoQuery();
            IEnumerable<BranchSpecificationInfoDto> result = await _sqlConnection.QueryAsync<BranchSpecificationInfoDto>(BranchSpecificationQuery, new { billId });

            return result;
        }
        private string GetBranchSpecificationSummayDtoQuery()
        {
            return @"select 
                	w.ReadingNumber,
                	w.SealNumber,
                	mp.Title as 'MeterProducerTitle',
                	md.Title as 'MeterDiameterTitle',
                	mt.Title as 'MeterProducerTitle',
                	mut.Title as 'MeterUseTypeTitle',
                	mi.Title as 'WaterMeterInstallationMethodTitle',
                	COUNT(s.InstallationDate) as N'SiphonCount',
                	sm.Title as 'SiphonMaterialTitle'
                from ClaimPool.WaterMeter w
                join ClaimPool.WaterMeterSiphon ws on ws.WaterMeterId=w.Id
                join ClaimPool.Siphon s on ws.SiphonId=s.Id
                join ClaimPool.MeterProducer mp on w.MeterProducerId=mp.Id
                join ClaimPool.MeterType mt on w.MeterTypeId=mt.Id
                join ClaimPool.MeterDiameter md on w.MeterDiameterId=md.Id
                join ClaimPool.MeterUseType mut on w.MeterUseTypeId =mut.Id
                join ClaimPool.WaterMeterInstallationMethod mi on w.WaterMeterInstallationMethodId=mi.Id
                join ClaimPool.SiphonMaterial sm on s.SiphonMaterialId=sm.Id
                where w.BillId=@billId
                group by
                    w.ReadingNumber,
                	w.SealNumber,
                	mp.Title ,
                	md.Title ,
                	mt.Title ,
                	mut.Title,
                	mi.Title,
                	sm.Title";

        }
    }
}
