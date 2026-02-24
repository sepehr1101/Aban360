using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using LiteDB;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    public sealed class MeterReadingDetailQueryService : AbstractBaseConnection, IMeterReadingDetailQueryService
    {
        public MeterReadingDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<MeterReadingDetailDataOutputDto>> Get(int flowImportedId)
        {
            string query = GetQuery();
            IEnumerable<MeterReadingDetailDataOutputDto> details = await _sqlReportConnection.QueryAsync<MeterReadingDetailDataOutputDto>(query, new { flowImportedId = flowImportedId });

            return details;
        }
        public async Task<MeterReadingDetailDataOutputDto> GetById(int id)
        {
            string query = GetSingleQuery();

            MeterReadingDetailDataOutputDto detail = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterReadingDetailDataOutputDto>(query, new { id = id });
            if (detail is null || detail.Id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidId);
            }
            return detail;
        }
    
        private string GetQuery()
        {
            return @"Select 
	                     m.Id,
	                     m.FlowImportedId,
	                     m.ZoneId,
	                     m.CustomerNumber,
	                     m.ReadingNumber,
	                     m.BillId,
	                     m.AgentCode,
	                     m.CurrentCounterStateCode,
	                     m.PreviousDateJalali,
	                     m.CurrentDateJalali,
	                     m.PreviousNumber,
	                     m.CurrentNumber,
	                     m.ExcludedByUserId, 
	                     m.ExcludedDateTime,
	                     m.InsertByUserId,
	                     m.InsertDateTime,
	                     m.RemovedByUserId,
	                     m.RemovedDateTime,
	                     m.BranchTypeId,
	                     t7.C1 as BranchTypeTitle,
	                     m.UsageId,
	                     t41.C1 as UsageTitle,
	                     m.ConsumptionUsageId,
	                     m.CommercialUnit,
	                     m.CommercialUnit,
	                     m.OtherUnit,
	                     m.EmptyUnit,
	                     m.WaterInstallationDateJalali,
	                     m.SewageInstallationDateJalali,
	                     m.WaterRegisterDate,
	                     m.SewageRegisterDate,
	                     m.WaterCount,
	                     m.SewageCalcState,
	                     m.ContractualCapacity,
	                     m.HouseholdNumber,
	                     m.HouseholdDate,
	                     m.VillageId,
	                     m.IsSpecial,
	                     m.MeterDiameterId,
	                     m.VirtualCategoryId,
	                     m.BodySerial, TavizDateJalali,
	                     m.TavizCause,
	                     m.TavizRegisterDateJalali,
	                     m.TavizNumber,
	                     m.LastMeterDateJalali,
	                     m.LastMeterNumber,
	                     m.LastMonthlyConsumption,
	                     m.LastCounterStateCode,
	                     m.LastSumItems,
	                     m.SumItems,
	                     m.SumItemsBeforeDiscount,
	                     m.DiscountSum,
	                     m.Consumption,
	                     m.MonthlyConsumption
                     From Atlas.dbo.MeterReadingDetail m
					 Left Join [Db70].dbo.T7 t7
						On m.BranchTypeId=t7.C0
					Left Join [Db70].dbo.T41 t41
						On m.UsageId=t41.C0
                     Where 
                        m.FlowImportedId=@flowImportedId AND
					    m.RemovedByUserId IS NULL";
        }
        private string GetSingleQuery()
        {
            return @"Select *
                     From Atlas.dbo.MeterReadingDetail
                     Where Id=@id";
        }
    }
}