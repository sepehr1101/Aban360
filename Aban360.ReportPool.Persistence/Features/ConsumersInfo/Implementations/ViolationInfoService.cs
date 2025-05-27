using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ViolationInfoService : AbstractBaseConnection, IViolationInfoService
    {
        public ViolationInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<ViolationInfoDto>> GetInfo(string billId)
        {
            string ViolationQuery = GetViolationSummayDtoQuery();
            // IEnumerable<ViolationInfoDto> result = await _sqlConnection.QueryAsync<ViolationInfoDto>(ViolationQuery, new { billId });
            IEnumerable<ViolationInfoDto> result = GetFakeViolationDtoQuery();

            return result;
        }
        private string GetViolationSummayDtoQuery()
        {
            return @"";
        }
        
        private IEnumerable<ViolationInfoDto> GetFakeViolationDtoQuery()
        {
            IEnumerable<ViolationInfoDto> violationInfoDtos = new List<ViolationInfoDto>()
            {
                new ViolationInfoDto()
                {
                    WaterViolationType="پمپ مستقیم",WastewaterViolationType="---",
                    PenaltyAmount="100000", OutstandingViolationBalance="12000",
                    ViolationDurationDays="3ماه",IllegalWaterConsumptionVolume=150, 
                    IllegalWastewaterDischargeVolume=150, ViolationStartDate="1402/10/05"
                },
                new ViolationInfoDto()
                {
                    WaterViolationType="دستکاری کنتور",WastewaterViolationType="---",
                    PenaltyAmount="7500000", OutstandingViolationBalance="423000",
                    ViolationDurationDays="3ماه",IllegalWaterConsumptionVolume=413, 
                    IllegalWastewaterDischargeVolume=413, ViolationStartDate="1402/09/20"
                },
                new ViolationInfoDto()
                {
                    WaterViolationType="حذف کنتور",WastewaterViolationType="---",
                    PenaltyAmount="520000", OutstandingViolationBalance="13000",
                    ViolationDurationDays="2ماه",IllegalWaterConsumptionVolume=112, 
                    IllegalWastewaterDischargeVolume=112, ViolationStartDate="1404/06/015"
                },
            };
            return violationInfoDtos;
        }
    }
}
