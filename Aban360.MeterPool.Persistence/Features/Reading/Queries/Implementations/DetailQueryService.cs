using Aban360.Common.Db.Dapper;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Implementations
{
    internal sealed class DetailQueryService : AbstractBaseConnection, IDetailQueryService
    {
        public DetailQueryService(IConfiguration configuration)
                : base(configuration)
        {
        }

        public async Task<IEnumerable<DetailGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query);
            return result;
        }
        public async Task<IEnumerable<DetailGetDto>> Get(Guid id)
        {
            string query = GetByIdQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query, new { id });
            return result;
        }
		public async Task<IEnumerable<DetailGetDto>> Get(IEnumerable<Guid> ids)
        {
            string query = GetByIdsQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query, new { ids });
            return result;
        }
		public async Task<IEnumerable<DetailGetDto>> GetByTrackIds(IEnumerable<Guid> ids)
        {
            string query = GetByTrackIdsQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query, new { TrackIds=ids });
            return result;
        }
        public async Task<IEnumerable<DetailGetDto>> GetUnComplete(long trackNumber)
        {
            string query = GetByUnCompleteByTrackNumberQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query, new { trackNumber });
            return result;
        }
        private string GetQuery()
        {
            return $@"Select 
						d.Id,
						d.TrackingId ,
						d.TrackNumber ,
						d.BillId ,
						d.CustomerNumber ,
						d.ReadingNumber ,
						d.FirstName ,
						d.SureName ,
						d.Address ,
						d.Plaque ,
						d.UsageId ,
						t41.C1 UsageTitle ,
						d.DomesticUnit ,
						d.CommercialUnit ,
						d.OtherUnit ,
						d.MeterDiameterId ,
						t5.C1 MeterDiameterTitle,
						d.SiphonDiameterId ,
						d.PostalCode ,
						d.PreNumber ,
						d.PreDateJalali ,
						d.PreDateTime ,
						d.PreConsumptionAverage ,
						d.PreCounterStateCode ,
						d.BodySerial ,
						d.MeterInstallationDateJalali,
						d.ChangeDateJalali ,
						d.ChangeNumber ,
						d.ContractualCapacity ,
						d.MobileNumber ,
						d.DeletionStateId ,
						d.BranchTypeId ,
						d.CounterNumber ,
						d.CurrentCounterStateCode ,
						d.PossibleAddress ,
						d.PossibleBodySerial ,
						d.PossibleReadingNumber ,
						d.PossibleMobileNumber ,
						d.PossiblePhoneNumber ,
						d.PossibleDomesticUnit ,
						d.PossibleCommercialUnit ,
						d.PossibleOtherUnit ,
						d.PossibleUsageId ,
						d.PossibleEmptyUnit ,
						d.OffLoadDateTime ,
						d.OffloadDateJalali ,
						d.OffLoadTime ,
						d.OffLoadBy ,
						d.Y ,
						d.X ,
						d.GisAccuracy ,
						d.ImageCount ,
						d.CurrentConsumption ,
						d.CurrentConsumptionAverage,
						d.DateDifference ,
						d.CounterNumberShown ,
						d.ExcludedForBazdid ,
						d.ExcludedForEslah ,
						d.JsonItems ,
						d.ConsumptionStateId ,
						d.Description ,
						d.IsCalculated ,
						d.CalculationErrorDescription ,
						d.D1 ,
						d.D2 ,
						d.CounterStatePosition ,
						d.AttemptCount ,
						d.IsLocked ,
						d.LocationDateTime ,
						d.PhoneDateTime ,
						d.FatherName ,
						d.OldReadingNumber ,
						d.OldCustomerNumber ,
						d.MobilesNumber ,
						d.WaterDebt,
						d.ModifierUserId ,
						d.GuildId ,
						d.SiphonInstallationDateJalali ,
						d.PreGuildId ,
						d.HighLowStateId ,
						d.ExcludeBy ,
						d.ExcludeDateTime ,
						d.ExcludeCauseId ,
						d.ExcludeCauseTitle
					From atlas.reading.Detail d
					Left Join Db70.dbo.T41 t41
						ON d.UsageId=t41.C0
					Left Join Db70.dbo.T5 t5
						ON d.MeterDiameterId=t5.C0 
					Where d.ExcludeBy IS NULL";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
						d.Id,
						d.TrackingId ,
						d.TrackNumber ,
						d.BillId ,
						d.CustomerNumber ,
						d.ReadingNumber ,
						d.FirstName ,
						d.SureName ,
						d.Address ,
						d.Plaque ,
						d.UsageId ,
						t41.C1 UsageTitle ,
						d.DomesticUnit ,
						d.CommercialUnit ,
						d.OtherUnit ,
						d.MeterDiameterId ,
						t5.C1 MeterDiameterTitle,
						d.SiphonDiameterId ,
						d.PostalCode ,
						d.PreNumber ,
						d.PreDateJalali ,
						d.PreDateTime ,
						d.PreConsumptionAverage ,
						d.PreCounterStateCode ,
						d.BodySerial ,
						d.MeterInstallationDateJalali,
						d.ChangeDateJalali ,
						d.ChangeNumber ,
						d.ContractualCapacity ,
						d.MobileNumber ,
						d.DeletionStateId ,
						d.BranchTypeId ,
						d.CounterNumber ,
						d.CurrentCounterStateCode ,
						d.PossibleAddress ,
						d.PossibleBodySerial ,
						d.PossibleReadingNumber ,
						d.PossibleMobileNumber ,
						d.PossiblePhoneNumber ,
						d.PossibleDomesticUnit ,
						d.PossibleCommercialUnit ,
						d.PossibleOtherUnit ,
						d.PossibleUsageId ,
						d.PossibleEmptyUnit ,
						d.OffLoadDateTime ,
						d.OffloadDateJalali ,
						d.OffLoadTime ,
						d.OffLoadBy ,
						d.Y ,
						d.X ,
						d.GisAccuracy ,
						d.ImageCount ,
						d.CurrentConsumption ,
						d.CurrentConsumptionAverage,
						d.DateDifference ,
						d.CounterNumberShown ,
						d.ExcludedForBazdid ,
						d.ExcludedForEslah ,
						d.JsonItems ,
						d.ConsumptionStateId ,
						d.Description ,
						d.IsCalculated ,
						d.CalculationErrorDescription ,
						d.D1 ,
						d.D2 ,
						d.CounterStatePosition ,
						d.AttemptCount ,
						d.IsLocked ,
						d.LocationDateTime ,
						d.PhoneDateTime ,
						d.FatherName ,
						d.OldReadingNumber ,
						d.OldCustomerNumber ,
						d.MobilesNumber ,
						d.WaterDebt,
						d.ModifierUserId ,
						d.GuildId ,
						d.SiphonInstallationDateJalali ,
						d.PreGuildId ,
						d.HighLowStateId ,
						d.ExcludeBy ,
						d.ExcludeDateTime ,
						d.ExcludeCauseId ,
						d.ExcludeCauseTitle
					From atlas.reading.Detail d
					Left Join Db70.dbo.T41 t41
						ON d.UsageId=t41.C0
					Left Join Db70.dbo.T5 t5
						ON d.MeterDiameterId=t5.C0 
					Where 
						d.ExcludeBy IS NULL AND
						d.Id = @Id";
        }
        private string GetByIdsQuery()
        {
            return $@"Select 
						d.Id,
						d.TrackingId ,
						d.TrackNumber ,
						d.BillId ,
						d.CustomerNumber ,
						d.ReadingNumber ,
						d.FirstName ,
						d.SureName ,
						d.Address ,
						d.Plaque ,
						d.UsageId ,
						t41.C1 UsageTitle ,
						d.DomesticUnit ,
						d.CommercialUnit ,
						d.OtherUnit ,
						d.MeterDiameterId ,
						t5.C1 MeterDiameterTitle,
						d.SiphonDiameterId ,
						d.PostalCode ,
						d.PreNumber ,
						d.PreDateJalali ,
						d.PreDateTime ,
						d.PreConsumptionAverage ,
						d.PreCounterStateCode ,
						d.BodySerial ,
						d.MeterInstallationDateJalali,
						d.ChangeDateJalali ,
						d.ChangeNumber ,
						d.ContractualCapacity ,
						d.MobileNumber ,
						d.DeletionStateId ,
						d.BranchTypeId ,
						d.CounterNumber ,
						d.CurrentCounterStateCode ,
						d.PossibleAddress ,
						d.PossibleBodySerial ,
						d.PossibleReadingNumber ,
						d.PossibleMobileNumber ,
						d.PossiblePhoneNumber ,
						d.PossibleDomesticUnit ,
						d.PossibleCommercialUnit ,
						d.PossibleOtherUnit ,
						d.PossibleUsageId ,
						d.PossibleEmptyUnit ,
						d.OffLoadDateTime ,
						d.OffloadDateJalali ,
						d.OffLoadTime ,
						d.OffLoadBy ,
						d.Y ,
						d.X ,
						d.GisAccuracy ,
						d.ImageCount ,
						d.CurrentConsumption ,
						d.CurrentConsumptionAverage,
						d.DateDifference ,
						d.CounterNumberShown ,
						d.ExcludedForBazdid ,
						d.ExcludedForEslah ,
						d.JsonItems ,
						d.ConsumptionStateId ,
						d.Description ,
						d.IsCalculated ,
						d.CalculationErrorDescription ,
						d.D1 ,
						d.D2 ,
						d.CounterStatePosition ,
						d.AttemptCount ,
						d.IsLocked ,
						d.LocationDateTime ,
						d.PhoneDateTime ,
						d.FatherName ,
						d.OldReadingNumber ,
						d.OldCustomerNumber ,
						d.MobilesNumber ,
						d.WaterDebt,
						d.ModifierUserId ,
						d.GuildId ,
						d.SiphonInstallationDateJalali ,
						d.PreGuildId ,
						d.HighLowStateId ,
						d.ExcludeBy ,
						d.ExcludeDateTime ,
						d.ExcludeCauseId ,
						d.ExcludeCauseTitle
					From atlas.reading.Detail d
					Left Join Db70.dbo.T41 t41
						ON d.UsageId=t41.C0
					Left Join Db70.dbo.T5 t5
						ON d.MeterDiameterId=t5.C0 
					Where d.Id IN @Ids";
        }
		private string GetByTrackIdsQuery()
        {
            return $@"Select 
						d.Id,
						d.TrackingId ,
						d.TrackNumber ,
						d.BillId ,
						d.CustomerNumber ,
						d.ReadingNumber ,
						d.FirstName ,
						d.SureName ,
						d.Address ,
						d.Plaque ,
						d.UsageId ,
						t41.C1 UsageTitle ,
						d.DomesticUnit ,
						d.CommercialUnit ,
						d.OtherUnit ,
						d.MeterDiameterId ,
						t5.C1 MeterDiameterTitle,
						d.SiphonDiameterId ,
						d.PostalCode ,
						d.PreNumber ,
						d.PreDateJalali ,
						d.PreDateTime ,
						d.PreConsumptionAverage ,
						d.PreCounterStateCode ,
						d.BodySerial ,
						d.MeterInstallationDateJalali,
						d.ChangeDateJalali ,
						d.ChangeNumber ,
						d.ContractualCapacity ,
						d.MobileNumber ,
						d.DeletionStateId ,
						d.BranchTypeId ,
						d.CounterNumber ,
						d.CurrentCounterStateCode ,
						d.PossibleAddress ,
						d.PossibleBodySerial ,
						d.PossibleReadingNumber ,
						d.PossibleMobileNumber ,
						d.PossiblePhoneNumber ,
						d.PossibleDomesticUnit ,
						d.PossibleCommercialUnit ,
						d.PossibleOtherUnit ,
						d.PossibleUsageId ,
						d.PossibleEmptyUnit ,
						d.OffLoadDateTime ,
						d.OffloadDateJalali ,
						d.OffLoadTime ,
						d.OffLoadBy ,
						d.Y ,
						d.X ,
						d.GisAccuracy ,
						d.ImageCount ,
						d.CurrentConsumption ,
						d.CurrentConsumptionAverage,
						d.DateDifference ,
						d.CounterNumberShown ,
						d.ExcludedForBazdid ,
						d.ExcludedForEslah ,
						d.JsonItems ,
						d.ConsumptionStateId ,
						d.Description ,
						d.IsCalculated ,
						d.CalculationErrorDescription ,
						d.D1 ,
						d.D2 ,
						d.CounterStatePosition ,
						d.AttemptCount ,
						d.IsLocked ,
						d.LocationDateTime ,
						d.PhoneDateTime ,
						d.FatherName ,
						d.OldReadingNumber ,
						d.OldCustomerNumber ,
						d.MobilesNumber ,
						d.WaterDebt,
						d.ModifierUserId ,
						d.GuildId ,
						d.SiphonInstallationDateJalali ,
						d.PreGuildId ,
						d.HighLowStateId ,
						d.ExcludeBy ,
						d.ExcludeDateTime ,
						d.ExcludeCauseId ,
						d.ExcludeCauseTitle
					From atlas.reading.Detail d
					Left Join Db70.dbo.T41 t41
						ON d.UsageId=t41.C0
					Left Join Db70.dbo.T5 t5
						ON d.MeterDiameterId=t5.C0 
					Where 
						d.ExcludeBy IS NULL AND
						d.TrackingId IN @TrackIds";
        }
        private string GetByUnCompleteByTrackNumberQuery()
        {
            return $@"Select 
						d.Id,
						d.TrackingId ,
						d.TrackNumber ,
						d.BillId ,
						d.CustomerNumber ,
						d.ReadingNumber ,
						d.FirstName ,
						d.SureName ,
						d.Address ,
						d.Plaque ,
						d.UsageId ,
						t41.C1 UsageTitle ,
						d.DomesticUnit ,
						d.CommercialUnit ,
						d.OtherUnit ,
						d.MeterDiameterId ,
						t5.C1 MeterDiameterTitle,
						d.SiphonDiameterId ,
						d.PostalCode ,
						d.PreNumber ,
						d.PreDateJalali ,
						d.PreDateTime ,
						d.PreConsumptionAverage ,
						d.PreCounterStateCode ,
						d.BodySerial ,
						d.MeterInstallationDateJalali,
						d.ChangeDateJalali ,
						d.ChangeNumber ,
						d.ContractualCapacity ,
						d.MobileNumber ,
						d.DeletionStateId ,
						d.BranchTypeId ,
						d.CounterNumber ,
						d.CurrentCounterStateCode ,
						d.PossibleAddress ,
						d.PossibleBodySerial ,
						d.PossibleReadingNumber ,
						d.PossibleMobileNumber ,
						d.PossiblePhoneNumber ,
						d.PossibleDomesticUnit ,
						d.PossibleCommercialUnit ,
						d.PossibleOtherUnit ,
						d.PossibleUsageId ,
						d.PossibleEmptyUnit ,
						d.OffLoadDateTime ,
						d.OffloadDateJalali ,
						d.OffLoadTime ,
						d.OffLoadBy ,
						d.Y ,
						d.X ,
						d.GisAccuracy ,
						d.ImageCount ,
						d.CurrentConsumption ,
						d.CurrentConsumptionAverage,
						d.DateDifference ,
						d.CounterNumberShown ,
						d.ExcludedForBazdid ,
						d.ExcludedForEslah ,
						d.JsonItems ,
						d.ConsumptionStateId ,
						d.Description ,
						d.IsCalculated ,
						d.CalculationErrorDescription ,
						d.D1 ,
						d.D2 ,
						d.CounterStatePosition ,
						d.AttemptCount ,
						d.IsLocked ,
						d.LocationDateTime ,
						d.PhoneDateTime ,
						d.FatherName ,
						d.OldReadingNumber ,
						d.OldCustomerNumber ,
						d.MobilesNumber ,
						d.WaterDebt,
						d.ModifierUserId ,
						d.GuildId ,
						d.SiphonInstallationDateJalali ,
						d.PreGuildId ,
						d.HighLowStateId ,
						d.ExcludeBy ,
						d.ExcludeDateTime ,
						d.ExcludeCauseId ,
						d.ExcludeCauseTitle
					From atlas.reading.Detail d
					Left Join Db70.dbo.T41 t41
						ON d.UsageId=t41.C0
					Left Join Db70.dbo.T5 t5
						ON d.MeterDiameterId=t5.C0 
					Where 
						d.ExcludeBy IS NULL AND
						d.OffLoadDateTime IS NULL AND
						d.TrackNumber = @TrackNumber";
        }
    }
}
