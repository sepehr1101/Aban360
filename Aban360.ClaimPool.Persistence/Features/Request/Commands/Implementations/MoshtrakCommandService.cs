using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    public sealed class MoshtrakCommandService //: AbstractBaseConnection, IMoshtrakCommandService
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDbTransaction _transaction;
        public MoshtrakCommandService(
            SqlConnection sqlConnection,
            IDbTransaction transaction)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.NotNull(nameof(sqlConnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Update(MoshtrkUpdateDto input, string dbName)
        {
            string command = GetUpdateCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, input);
            if (recordCount != 1)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }

        private string GetUpdateCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.moshtrak
						Set 
							par_no=@StringTrackNumber,
							name=@FirstName,
							family=@Surname,
							father_nam=@FatherName ,
							arse=@Premises,
							aian=@ImprovementOverall ,
							aian_mas=@ImprovementDomestic,
							aian_tej=@ImprovementCommertial ,
							tedad_mas=@CommertialUnit,
							tedad_tej=@DomesticUnit	,
							tedad_vahd=@OtherUnit,
							noe_va=@BranchTypeId,
							mojavz=@IsNonPermanent,
							enshab=@UsageId,
							master_sif=@MainSiphon,
							sif_1=@Siphon100,
							sif_2=@Siphon125,
							sif_3=@Siphon150,
							sif_4=@Siphon200,
							sif_mosh_1=@CommonSiphon,
							cod_enshab=@MeterDiameterId,
							date_ask=@CurrentDateJalali,
							address=@Address ,
							cod_takh=@DiscountTypeId,
							ted_takh=@DiscountCount ,
							meli_cod=@NationalCode,
							post_cod=@PostalCode,
							s0=@s0,
							s1=@s1,
							s2=@s2,
							s3=@s3,
							s4=@s4,
							s5=@s5,
							s8=@s8,
							s9=@s9,
							s10=@s10,
							s11=@s11,
							s12=@s12,
							s13=@s13,
							s14=@s14,
							s15=@s15,
							s16=@s16,
							s17=@s17,
							s18=@s18,
							s19=@s19,
							s20=@s20,
							s21=@s21,
							s22=@s22,
							s23=@s23,
							s24=@s24,
							s25=@s25,
							s26=@s26,
							s27=@s27,
							s28=@s28,
							s29=@s29,
							s30=@s30,
							s31=@s31,
							s32=@s32,
							s33=@s33,
							s34=@s34,
							s35=@s35,
							s36=@s36,
							s37=@s37,
							s38=@s38,
							s39=@s39,
							s40=@s40,
							s41=@s41,
							s42=@s42,
							s43=@s43,
							s44=@s44,
							s45=@s45,
							s46=@s46,
							s47=@s47,
							s48=@s48,
							CounterType=@CounterType,
							fix_mas=@ContractualCapacity ,
							zarib_f=@HouseValue
						Where TrackingNumber=@TrackNumber";
        }
    }
}
