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
        private readonly IDbConnection _sqlConnection;
        private readonly IDbTransaction _transaction;
        public MoshtrakCommandService(
            IDbConnection sqlConnection,
            IDbTransaction transaction)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.NotNull(nameof(sqlConnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(MoshtrakCreateDto input, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, input, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertTracking);
            }
        }
        public async Task Update(MoshtrkUpdateDto input, string dbName)
        {
            string command = GetUpdateCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, input, _transaction);
            if (recordCount != 1)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
		public async Task Update(MoshtrakUpdateInfoDto input, string dbName)
        {
            string command = GetUpdateInfoCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, input, _transaction);
            if (recordCount != 1)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }
        public async Task UpdateSabt(MoshtrakSabtUpdateDto input, string dbName)
        {
            string command = GetUpdateSabtCommand(dbName);
            int recordCount = await _sqlConnection.ExecuteAsync(command, input, _transaction);
            if (recordCount != 1)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateMoshtrakin);
            }
        }

        //   private string GetInsertCommand(string dbName)
        //   {
        //       return $@"Insert [{dbName}].dbo.moshtrak(
        //	town,radif,par_no,name,family,
        //	father_nam,date_ask,address,s0,s1,
        //	meli_cod,post_cod,phone_no,mobile,ICT_CO,
        //	TrackingNumber,NeighbourBillID)
        //Values(
        //	@ZoneId ,@CustomerNumber ,@StringTrackNumber ,@FirstName ,@Surname	,
        //	@FatherName ,@CurrentDateJalali ,@Address ,@s0 ,@s1 ,
        //	@NationalCode ,@PostalCode ,@PhoneNumber ,@MobileNumber ,@InsertWayTitle ,
        //	@TrackNumber ,@NeighbourBillId) ";
        //   }
        private string GetInsertCommand(string dbName)
        {
            return $@"Insert [{dbName}].dbo.moshtrak(
						town,radif,par_no,name,family,
						father_nam,date_ask,date,address,
						meli_cod,post_cod,phone_no,mobile,ICT_CO,
						TrackingNumber,NeighbourBillID,
					    eshtrak,arse,aian,aian_mas,
					    aian_tej,tedad_mas,tedad_tej,tedad_vahd,noe_va,
					    enshab,master_sif,sif_1,sif_2,sif_3,
					    sif_4,sif_mosh_1,cod_enshab,
					    s0,s2,s1,s3,s4,s5,
					    s8,s9,s10,s11,s12,
					    s13,s14,s15,s16,s17,
					    s18,s19,s20,s21,s22,
					    s23,s24,s25,s26,s27,
					    s28,s29,s30,s31,s32,
					    s33,s34,s35,s36,s37,
					    s38,s39,s40,s41,s42,
					    s43,s44,s45,s46,s47,
					    s48,edareh_k,fix_mas,BLOCK_COD,Kargozari)
					Values(
						@ZoneId ,@CustomerNumber ,@StringTrackNumber ,@FirstName ,@Surname	,
						@FatherName ,@CurrentDateJalali ,@CurrentDateJalali ,@Address ,
						@NationalCode ,@PostalCode ,@PhoneNumber ,@MobileNumber ,@InsertWayTitle ,
						@TrackNumber ,@NeighbourBillId,
						@ReadingNumber ,@Premises ,@ImprovementOverall ,@ImprovementDomestic ,
						@ImprovementCommertial ,@DomesticUnit ,@CommertialUnit ,@OtherUnit ,@BranchTypeId ,
						@MeterDiameterId ,@MainSiphon ,@Siphon100 ,@Siphon125 ,@Siphon150 ,
						@Siphon200 ,@CommonSiphon ,@UsageId ,
						@s0 ,@s1 ,@s2 ,@s3 ,@s4 ,@s5 ,
						@s8 ,@s9 ,@s10 ,@s11 ,@s12 ,
						@s13 ,@s14 ,@s15 ,@s16 ,@s17 ,
						@s18 ,@s19 ,@s20 ,@s21 ,@s22 ,
						@s23 ,@s24 ,@s25 ,@s26 ,@s27 ,
						@s28 ,@s29 ,@s30 ,@s31 ,@s32 ,
						@s33 ,@s34 ,@s35 ,@s36 ,@s37 ,
						@s38 ,@s39 ,@s40 ,@s41 ,@s42 ,
						@s43 ,@s44 ,@s45 ,@s46 ,@s47 ,
						@s48 ,@IsSpecial ,@ContractualCapacity ,@BlockId ,@BrokerId) ";
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
		private string GetUpdateInfoCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.moshtrak
						Set 
							name=@FirstName,
							family=@Surname,
							father_nam=@FatherName ,
							arse=@Premises,
							aian=@ImprovementOverall ,
							aian_mas=@ImprovementDomestic,
							aian_tej=@ImprovementCommercial ,
							tedad_mas=@CommercialUnit,
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
							address=@Address ,
							cod_takh=@DiscountTypeId,
							ted_takh=@DiscountCount ,
							meli_cod=@NationalCode,
							post_cod=@PostalCode,
							CounterType=@CounterType,
							fix_mas=@ContractualCapacity ,
							zarib_f=@HouseValue,
							edareh_k=@IsSpecial,
							C99=@NotificationMobile,
							sharh=@Description,
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
							s48=@s48
						Where TrackingNumber=@TrackNumber";
        }
        private string GetUpdateSabtCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.moshtrak
						Set sabt=@IsRegister , sharh=@Description
						Where TrackingNumber=@TrackNumber";
        }
    }
}
