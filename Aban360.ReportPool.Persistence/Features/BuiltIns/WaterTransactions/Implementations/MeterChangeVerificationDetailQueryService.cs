using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class MeterChangeVerificationDetailQueryService : AbstractBaseConnection, IMeterChangeVerificationDetailQueryService
    {
        public MeterChangeVerificationDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto>> GetInfo(MeterChangeVerificationInputDto input)
        {
            string query = GetQuery(input.IsChangeDate);

            IEnumerable<MeterChangeVerificationDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<MeterChangeVerificationDetailDataOutputDto>(query, input);
            MeterChangeVerificationHeaderOutputDto header = new MeterChangeVerificationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.DistinctBy(x => x.BillId)?.Count() ?? 0,
                Title = ReportLiterals.MeterChangeVerification,
            };

            var result = new ReportOutput<MeterChangeVerificationHeaderOutputDto, MeterChangeVerificationDetailDataOutputDto>(ReportLiterals.MeterChangeVerification, header, data);
            return result;
        }

        private string GetQuery(bool isChangeDate)
        {
			string dateCondition = isChangeDate ? "ChangeDateJalali" : "RegisterDateJalali";
            return @$";With MeterChangeInfo As(	
					Select 
						m.BodySerial ChangeBodySerial,
						m.ChangeDateJalali,
						m.ChangeCauseId,
						m.ChangeCauseTitle,
						m.MeterNumber ChangeMeterNumber,
						m.RegisterDateJalali ChangeRegisterDateJalali,
						b.*,
						Rn=ROW_NUMBER() OVER(PARTITION BY m.{dateCondition},b.ZoneId,b.CustomerNumber ORDER BY m.{dateCondition} Desc,b.RegisterDay ASC )
					From CustomerWarehouse.dbo.MeterChange m
					Join CustomerWarehouse.dbo.Bills b
						On m.ZoneId=b.ZoneId AND m.CustomerNumber=b.CustomerNumber AND b.RegisterDay>m.{dateCondition}
					Where 
						m.ZoneId IN @ZoneIds AND
						m.{dateCondition} BETWEEN @FromDateJalali AND @ToDateJalali AND
						b.CounterStateCode NOT IN (4,7,8)
				)
				Select  
					m.ZoneId,
					m.ZoneTitle,
					m.CustomerNumber,
					m.BillId,
					m.ReadingNumber,
					m.ChangeDateJalali,
					m.ChangeRegisterDateJalali,
					m.CounterStateCode,
					cv.Title CounterStateTitle,
					m.RegisterDay BillRegisterDay,
					m.PreviousDay PreviousDateJalali,
					m.PreviousNumber PreviousNumber,
					m.NextDay CurrentDateJalali,
					m.NextNumber CurrentNumber,
					m.BranchType BranchTypeTitle,
					m.BranchTypeId,
					m.UsageId, 
					m.UsageTitle,
					m.Consumption,
					m.ConsumptionAverage,
					m.Duration
				From MeterChangeInfo m
				Join Db70.dbo.CounterVaziat cv
					ON m.CounterStateCode = cv.MoshtarakinId
				Where 
					Rn=1 AND
					m.CounterStateCode <> 2;";
        }
    }
}
