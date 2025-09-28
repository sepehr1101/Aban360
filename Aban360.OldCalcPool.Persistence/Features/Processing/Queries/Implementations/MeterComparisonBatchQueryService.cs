using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class MeterComparisonBatchQueryService : AbstractBaseConnection, IMeterComparisonBatchQueryService
    {
        string reportTitle = "مغایرت محاسبه";
        public MeterComparisonBatchQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataWithCustomerInfoOutputDto>> Get(MeterComparisonBatchInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string perviousBillsDataQueryString = GetPreviousBillsDataQuery(dbName, input.IsRegisterDateJalali);
            var @params = new
            {
                fromDateJalali = input.FromDateJalali,
                toDateJalali = input.ToDateJalali,
                zoneId = input.ZoneId,
            };
            IEnumerable<MeterComparisonBatchDataWithCustomerInfoOutputDto> details = await _sqlReportConnection.QueryAsync<MeterComparisonBatchDataWithCustomerInfoOutputDto>(perviousBillsDataQueryString, @params);
            MeterComparisonBatchHeaderOutputDto summary = new()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = details?.Count() ?? 00,
                ZoneTitle = details?.FirstOrDefault()?.ZoneTitle??"-",
                SumPreviousAmount = details?.Sum(m => m.PreviousAmount) ?? 0,
            };
            ReportOutput<MeterComparisonBatchHeaderOutputDto, MeterComparisonBatchDataWithCustomerInfoOutputDto> result = new(reportTitle, summary, details);
            return result;
        }

        private string GetPreviousBillsDataQuery(string dbName, bool isRegisterDateJalali)
        {
            string byRegisterDate = $"b.date_bed BETWEEN @fromDatejalali AND @toDateJalali ";
            string byPreviousNextDate = $"b.pri_date>=@fromDateJalali AND b.today_date<=@toDateJalali";

            string conditionDateQuery = isRegisterDateJalali ? byRegisterDate : byPreviousNextDate;

            return @$"Select
                        t51.C2 as ZoneTitle,
                    	m.town as ZoneId,
                    	m.radif as Radif,
						Trim(m.bill_id) as BillId,
                    	b.noe_va as BranchType,
                    	b.cod_enshab as UsageId,
                    	b.tedad_mas as DomesticUnit,
                    	b.tedad_tej as CommertialUnit,
                    	b.tedad_vahd as OtherUnit,
                    	m.inst_ab as WaterInstallationDateJalali,
                    	m.inst_fas as SewageInstallationDateJalali,
                    	m.n_ab as WaterCount,
                    	m.n_faz as SewageCalcState,
						b.fix_mas as ContractualCapacity,
                        b.ted_khane as HouseholdNumber,
						b.eshtrak as ReadingNumber,
                        m.VillageId as VillageId,
						b.edareh_k as IsSpecial,
						b.enshab as MeterDiameterId,
						b.Khali_s as EmptyUnit,
						b.pri_date as PreviousDateJalali,
						b.today_date as CurrentDateJalali,
						b.pri_no as PreviousMeterNumber,
						b.today_no as CurrentMeterNumber,
						b.baha as PreviousAmount,
						ISNULL(k.baha,0) as PreviousDiscount
                    From [{dbName}].dbo.bed_bes b
					Join [{dbName}].dbo.members m	
						On b.town=m.town AND b.radif=m.radif
                    Left Outer Join [{dbName}].dbo.kasr_ha k
						On b.town=k.TOWN AND b.radif=k.radif AND b.barge=k.barge AND b.date_bed=k.date_bed
					Join [Db70].dbo.T51 t51 
						On m.town=t51.C0
                    Where
                    	m.town=@zoneId AND 
						b.cod_vas IN (0) AND
						{conditionDateQuery}";
            //b.date_bed BETWEEN @fromDatejalali AND @toDateJalali
        }
    }
}
