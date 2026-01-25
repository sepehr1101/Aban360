using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    internal sealed class MembersCommandService : AbstractBaseConnection, IMembersCommandService
    {
        public MembersCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Update(CustomerUpdateDto updateDto)
        {
            //string dbName = GetDbName(updateDto.ZoneId);
            string dbName = "Atlas";
            string command = GetUpdateCommand(dbName);

            if (_sqlReportConnection.State != System.Data.ConnectionState.Open)
                await _sqlReportConnection.OpenAsync();

            var updateResult = await _sqlReportConnection.ExecuteAsync(command, updateDto);

        }
        private string GetUpdateCommand(string dbName)
        {
            return @$"UPDATE [{dbName}].dbo.members
                     SET 
                        eshtrak=@ReadingNumber,
	                    name=@FirstName,
	                    family=@SurName,
	                    father_nam=@FatherName,
	                    enshab=@MeterDiamterId,
	                    cod_enshab=@UsageSellId,
	                    tedad_vahd=@OtherUnit,
	                    tedad_mas=@DomesticUnit,
	                    ted_khane=@HouseholdNumber,
	                    tedad_tej=@CommertialUnit,
	                    arse=@Premises,
	                    aian=@ImprovementOverall,
	                    aian_mas=@ImprovementDomestic,
	                    aian_tej=@ImprovementCommertial,
	                    ask_ab=@WaterRequestDateJalali,
	                    inst_ab=@WaterInstallationDateJalali,
	                    ask_fas=@SewageRequestDateJalali,
	                    inst_fas=@SewageInstallationDateJalali,
	                    address=@Address,
	                    pelak=@Plaque,
	                    edareh_k=@IsSpecial,
	                    noe_va=@BranchTypeId,
	                    master_sif=@MainSiphon,
	                    sif_1=@Siphon100,
	                    sif_2=@Siphon125,
	                    sif_3=@Siphon150,
	                    sif_4=@Siphon200,
	                    sif_5=@Siphon5,
	                    sif_6=@Siphon6,
	                    sif_7=@Siphon7,
	                    sif_8=@Siphon8,	
	                    fix_mas=@ContractualCapacity,
	                    group1=@UsageConsumptionId,
	                    operator=@Operator,
	                    POST_COD=@PostalCode,
	                    PHONE_NO=@PhoneNumber,
	                    MOBILE=@MobileNumber,
	                    MELI_COD=@NationalCode,
	                    Khali_s=@EmptyUnit,
	                    date_KHANE=@HouseholdDateJalali,
	                    X=@x,
	                    Y=@y,
	                    date_sabt=@ToDayDateJalali
                     WHERE 
                        id=@id AND
						bill_id=@billId AND
						town=@zoneId AND
						radif=@customerNumber ";
        }
    }

}
