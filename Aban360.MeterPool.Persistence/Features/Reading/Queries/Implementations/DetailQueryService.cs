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
		public async Task<IEnumerable<DetailGetDto>> GetByTrackIds(IEnumerable<Guid> ids)
        {
            string query = GetByTrackIdsQuery();
            IEnumerable<DetailGetDto> result = await _sqlReportConnection.QueryAsync<DetailGetDto>(query, new { TrackIds=ids });
            return result;
        }

        private string GetQuery()
        {
            return $@"Select 
						Id,
						TrackingId ,
						TrackNumber ,
						BillId ,
						CustomerNumber ,
						ReadingNumber ,
						FirstName ,
						SureName ,
						Address ,
						Plaque ,
						UsageId ,
						DomesticUnit ,
						CommercialUnit ,
						OtherUnit ,
						MeterDiameterId ,
						SiphonDiameterId ,
						PostalCode ,
						PreNumber ,
						PreDateJalali ,
						PreDateTime ,
						PreConsumptionAverage ,
						PreCounterStateCode ,
						BodySerial ,
						MeterInstallationDateJalali,
						ChangeDateJalali ,
						ChangeNumber ,
						ContractualCapacity ,
						MobileNumber ,
						DeletionStateId ,
						BranchTypeId ,
						CounterNumber ,
						CurrentCounterStateCode ,
						PossibleAddress ,
						PossibleBodySerial ,
						PossibleReadingNumber ,
						PossibleMobileNumber ,
						PossiblePhoneNumber ,
						PossibleDomesticUnit ,
						PossibleCommercialUnit ,
						PossibleOtherUnit ,
						PossibleUsageId ,
						PossibleEmptyUnit ,
						OffLoadDateTime ,
						OffloadDateJalali ,
						OffLoadTime ,
						OffLoadBy ,
						Y ,
						X ,
						GisAccuracy ,
						ImageCount ,
						CurrentConsumption ,
						CurrentConsumptionAverage,
						DateDifference ,
						CounterNumberShown ,
						ExcludedForBazdid ,
						ExcludedForEslah ,
						JsonItems ,
						ConsumptionStateId ,
						Description ,
						IsCalculated ,
						CalculationErrorDescription ,
						D1 ,
						D2 ,
						CounterStatePosition ,
						AttemptCount ,
						IsLocked ,
						LocationDateTime ,
						PhoneDateTime ,
						FatherName ,
						OldReadingNumber ,
						OldCustomerNumber ,
						MobilesNumber ,
						WaterDebt,
						ModifierUserId ,
						GuildId ,
						SiphonInstallationDateJalali ,
						PreGuildId ,
						HighLowStateId ,
						ExcludeBy ,
						ExcludeDateTime ,
						ExcludeCauseId ,
						ExcludeCauseTitle
					From atlas.reading.Detail
					Where ExcludeBy IS NULL";
        }
        private string GetByIdQuery()
        {
            return $@"Select 
						Id,
						TrackingId ,
						TrackNumber ,
						BillId ,
						CustomerNumber ,
						ReadingNumber ,
						FirstName ,
						SureName ,
						Address ,
						Plaque ,
						UsageId ,
						DomesticUnit ,
						CommercialUnit ,
						OtherUnit ,
						MeterDiameterId ,
						SiphonDiameterId ,
						PostalCode ,
						PreNumber ,
						PreDateJalali ,
						PreDateTime ,
						PreConsumptionAverage ,
						PreCounterStateCode ,
						BodySerial ,
						MeterInstallationDateJalali,
						ChangeDateJalali ,
						ChangeNumber ,
						ContractualCapacity ,
						MobileNumber ,
						DeletionStateId ,
						BranchTypeId ,
						CounterNumber ,
						CurrentCounterStateCode ,
						PossibleAddress ,
						PossibleBodySerial ,
						PossibleReadingNumber ,
						PossibleMobileNumber ,
						PossiblePhoneNumber ,
						PossibleDomesticUnit ,
						PossibleCommercialUnit ,
						PossibleOtherUnit ,
						PossibleUsageId ,
						PossibleEmptyUnit ,
						OffLoadDateTime ,
						OffloadDateJalali ,
						OffLoadTime ,
						OffLoadBy ,
						Y ,
						X ,
						GisAccuracy ,
						ImageCount ,
						CurrentConsumption ,
						CurrentConsumptionAverage,
						DateDifference ,
						CounterNumberShown ,
						ExcludedForBazdid ,
						ExcludedForEslah ,
						JsonItems ,
						ConsumptionStateId ,
						Description ,
						IsCalculated ,
						CalculationErrorDescription ,
						D1 ,
						D2 ,
						CounterStatePosition ,
						AttemptCount ,
						IsLocked ,
						LocationDateTime ,
						PhoneDateTime ,
						FatherName ,
						OldReadingNumber ,
						OldCustomerNumber ,
						MobilesNumber ,
						WaterDebt,
						ModifierUserId ,
						GuildId ,
						SiphonInstallationDateJalali ,
						PreGuildId ,
						HighLowStateId ,
						ExcludeBy ,
						ExcludeDateTime ,
						ExcludeCauseId ,
						ExcludeCauseTitle
					From atlas.reading.Detail
					Where 
						ExcludeBy IS NULL AND
						Id = @Id";
        }
        private string GetByTrackIdsQuery()
        {
            return $@"Select 
						Id,
						TrackingId ,
						TrackNumber ,
						BillId ,
						CustomerNumber ,
						ReadingNumber ,
						FirstName ,
						SureName ,
						Address ,
						Plaque ,
						UsageId ,
						DomesticUnit ,
						CommercialUnit ,
						OtherUnit ,
						MeterDiameterId ,
						SiphonDiameterId ,
						PostalCode ,
						PreNumber ,
						PreDateJalali ,
						PreDateTime ,
						PreConsumptionAverage ,
						PreCounterStateCode ,
						BodySerial ,
						MeterInstallationDateJalali,
						ChangeDateJalali ,
						ChangeNumber ,
						ContractualCapacity ,
						MobileNumber ,
						DeletionStateId ,
						BranchTypeId ,
						CounterNumber ,
						CurrentCounterStateCode ,
						PossibleAddress ,
						PossibleBodySerial ,
						PossibleReadingNumber ,
						PossibleMobileNumber ,
						PossiblePhoneNumber ,
						PossibleDomesticUnit ,
						PossibleCommercialUnit ,
						PossibleOtherUnit ,
						PossibleUsageId ,
						PossibleEmptyUnit ,
						OffLoadDateTime ,
						OffloadDateJalali ,
						OffLoadTime ,
						OffLoadBy ,
						Y ,
						X ,
						GisAccuracy ,
						ImageCount ,
						CurrentConsumption ,
						CurrentConsumptionAverage,
						DateDifference ,
						CounterNumberShown ,
						ExcludedForBazdid ,
						ExcludedForEslah ,
						JsonItems ,
						ConsumptionStateId ,
						Description ,
						IsCalculated ,
						CalculationErrorDescription ,
						D1 ,
						D2 ,
						CounterStatePosition ,
						AttemptCount ,
						IsLocked ,
						LocationDateTime ,
						PhoneDateTime ,
						FatherName ,
						OldReadingNumber ,
						OldCustomerNumber ,
						MobilesNumber ,
						WaterDebt,
						ModifierUserId ,
						GuildId ,
						SiphonInstallationDateJalali ,
						PreGuildId ,
						HighLowStateId ,
						ExcludeBy ,
						ExcludeDateTime ,
						ExcludeCauseId ,
						ExcludeCauseTitle
					From atlas.reading.Detail
					Where 
						ExcludeBy IS NULL AND
						TrackingId IN @TrackIds";
        }
   
	}
}
