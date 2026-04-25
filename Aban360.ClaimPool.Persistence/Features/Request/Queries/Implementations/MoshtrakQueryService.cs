using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class MoshtrakQueryService : AbstractBaseConnection, IMoshtrakQueryService
    {
        public MoshtrakQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task CheckOpenRequest(string nationalCode, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetHasOpenRequestByNationalCodeQuery(dbName);
            string? trackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { nationalCode });
            if (!string.IsNullOrWhiteSpace(trackNumber))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOpenRequest(trackNumber));
            }
        }
        public async Task CheckOpenRequest(int customerNumber, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetHasOpenRequestByCustomerNumberCodeQuery(dbName);
            string? trackNumber = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { customerNumber, zoneId });
            if (!string.IsNullOrWhiteSpace(trackNumber))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOpenRequest(trackNumber));
            }
        }
        public async Task<IEnumerable<MoshtrakOutputDto>> Get(MoshtrakGetDto inputDto, MoshtrakSearchTypeEnum searchType, bool hasException = true)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetInfoByConditionQuery(dbName, GetCondition(searchType));
            IEnumerable<MoshtrakOutputDto> result = await _sqlReportConnection.QueryAsync<MoshtrakOutputDto>(query, inputDto);
            if (hasException && !result.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidTrackNumber);
            }
            //foreach (var item in result)
            //{
            //    if (item.TrackNumber == 0)
            //    {
            //        throw new InvalidTrackingException(ExceptionLiterals.InvalidLastDbData(item.StringTrackNumber));
            //    }
            //}
            return result;
        }
        public async Task<IEnumerable<PreviousRequestGetDto>> GetAllRequestByCustomerNumber(ZoneIdAndCustomerNumber inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetAllRequestByCustomerNumber(dbName);
            IEnumerable<PreviousRequestGetDto> result = await _sqlReportConnection.QueryAsync<PreviousRequestGetDto>(query, inputDto);
            if (!result.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidBillId);
            }
            return result;
        }
        public async Task<MoshtrakOutputDto> Get(int id, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetInfoByIdQuery(dbName);
            MoshtrakOutputDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MoshtrakOutputDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidTrackNumber);
            }
            return result;
        }
        private string GetCondition(MoshtrakSearchTypeEnum searchType)
        {
            return searchType switch
            {
                MoshtrakSearchTypeEnum.ByTrackNumber => " trackingnumber=@TrackNumber ",
                MoshtrakSearchTypeEnum.ByCustomerNumber => " radif=@customerNumber AND town=@zoneId ",
                MoshtrakSearchTypeEnum.ByNationalCode => " TRIM(meli_cod)=@nationalCode ",
                _ => string.Empty,
            };
        }
        private string GetHasOpenRequestByNationalCodeQuery(string dbName)
        {
            return $@"Select TrackingNumber
                    From [{dbName}].dbo.Moshtrak 
                    Where 
                    	TRIM(meli_cod)=@NationalCode AND
                    	sabt=0";
        }
        private string GetHasOpenRequestByCustomerNumberCodeQuery(string dbName)
        {
            return $@"Select TrackingNumber
                    From [{dbName}].dbo.Moshtrak 
                    Where 
                    	radif=@customerNumber AND
						town=@zoneId AND
                    	sabt=0";
        }
        private string GetInfoByConditionQuery(string dbName, string condition)
        {
            return $@"Select
                        Id,
                    	town ZoneId,
                    	t51.C2 ZoneTitle,
                    	radif CustomerNumber,
                        TRIM(eshtrak) ReadingNumber,
                    	TRIM(name) FirstName,
                    	TRIM(family) Surname,
                    	TRIM(father_nam) FatherName,
                    	TRIM(meli_cod) NationalCode,
                    	TRIM(phone_no) PhoneNumber,
                    	TRIM(mobile) MobileNumber,
                    	date_ask RequestDateJalali,
                    	TRIM(address) Address,
                    	TRIM(post_cod)PostalCode,
                    	TRIM(NeighbourBillID) NeighbourBillId,
                    	TrackingNumber TrackNumber,
                        par_no	StringTrackNumber,
                        cod_enshab UsageId,
						t41.C1 UsageTitle,	
                        Sabt IsRegistered,
						arse Premises,
						aian ImprovementOverall,
						aian_mas ImprovementDomestic,
						aian_tej ImprovementCommercial,
						tedad_vahd OtherUnit,
						tedad_mas DomesticUnit,
						tedad_tej CommercialUnit,
						fix_mas ContractualCapacity,
						sif_1 Siphon100,
						sif_2 Siphon125,
						sif_3 Siphon150,
						sif_4 Siphon200,
						master_sif MainSiphon,
						sif_mosh_1 CommonSiphon,
						enshab MeterDiameterId ,
						t5.C2 MeterDiameterTitle ,
						cod_takh DiscountTypeId,
						t15.C1 DiscountTypeTitle,
						ted_takh DiscountCount,
						edareh_k IsSpecial,
						CounterType,
						TRIM(C99) NotificationMobile,
						TRIM(sharh) Description ,
                        zarib_f HouseValue,
                    	noe_va BranchTypeId,
                    	t7.c1 BranchTypeTitle,
                        mojavz IsNonPermanent,
                        TRIM(BLOCK_COD) BlockId,
                        Kargozari BrokerId,
                        s0,
                        s1,
                        s2,
                        s3,
                        s4,
                        s5,
                        s8,
                        s9,
                        s10,
                        s11,
                        s12,
                        s13,
                        s14,
                        s15,
                        s16,
                        s17,
                        s18,
                        s19,
                        s20,
                        s21,
                        s22,
                        s23,
                        s24,
                        s25,
                        s26,
                        s27,
                        s28,
                        s29,
                        s30,
                        s31,
                        s32,
                        s33,
                        s34,
                        s35,
                        s36,
                        s37,
                        s38,
                        s39,
                        s40,
                        s41,
                        s42,
                        s43,
                        s44,
                        s45,
                        s46,
                        s47,
                        s48
                    From [{dbName}].dbo.moshtrak 
                    Join Db70.dbo.T51 t51
                    	ON town=t51.C0
					Join Db70.dbo.T5 t5
						ON enshab=t5.C0
					Join Db70.dbo.T41 t41
						ON enshab=t41.C0
					Join Db70.dbo.T15 t15
						ON enshab=t15.C0
                    join [Db70].dbo.T7 t7
                    	On noe_va=t7.C0
                    where {condition}
                    Order By date_ask Desc";
        }
        private string GetInfoByIdQuery(string dbName)
        {
            return $@"Select
                        Id,
                    	town ZoneId,
                    	t51.C2 ZoneTitle,
                    	radif CustomerNumber,
                        TRIM(eshtrak) ReadingNumber,
                    	TRIM(name) FirstName,
                    	TRIM(family) Surname,
                    	TRIM(father_nam) FatherName,
                    	TRIM(meli_cod) NationalCode,
                    	TRIM(phone_no) PhoneNumber,
                    	TRIM(mobile) MobileNumber,
                    	date_ask RequestDateJalali,
                    	TRIM(address) Address,
                    	TRIM(post_cod)PostalCode,
                    	TRIM(NeighbourBillID) NeighbourBillId,
                    	TrackingNumber TrackNumber,
                        par_no	StringTrackNumber,
                        cod_enshab UsageId,
						t41.C1 UsageTitle,	
                        Sabt IsRegistered,
						arse Premises,
						aian ImprovementOverall,
						aian_mas ImprovementDomestic,
						aian_tej ImprovementCommercial,
						tedad_vahd OtherUnit,
						tedad_mas DomesticUnit,
						tedad_tej CommercialUnit,
						fix_mas ContractualCapacity,
						sif_1 Siphon100,
						sif_2 Siphon125,
						sif_3 Siphon150,
						sif_4 Siphon200,
						master_sif MainSiphon,
						sif_mosh_1 CommonSiphon,
						enshab MeterDiameterId ,
						t5.C2 MeterDiameterTitle ,
						cod_takh DiscountTypeId,
						t15.C1 DiscountTypeTitle,
						ted_takh DiscountCount,
						edareh_k IsSpecial,
						CounterType,
						TRIM(C99) NotificationMobile,
						TRIM(sharh) Description ,
                        zarib_f HouseValue,
                    	noe_va BranchTypeId,
                    	t7.c1 BranchTypeTitle,
                        mojavz IsNonPermanent,
                        TRIM(BLOCK_COD) BlockId,
                        Kargozari BrokerId,
                        s0,
                        s1,
                        s2,
                        s3,
                        s4,
                        s5,
                        s8,
                        s9,
                        s10,
                        s11,
                        s12,
                        s13,
                        s14,
                        s15,
                        s16,
                        s17,
                        s18,
                        s19,
                        s20,
                        s21,
                        s22,
                        s23,
                        s24,
                        s25,
                        s26,
                        s27,
                        s28,
                        s29,
                        s30,
                        s31,
                        s32,
                        s33,
                        s34,
                        s35,
                        s36,
                        s37,
                        s38,
                        s39,
                        s40,
                        s41,
                        s42,
                        s43,
                        s44,
                        s45,
                        s46,
                        s47,
                        s48
                    From [{dbName}].dbo.moshtrak 
                    Join Db70.dbo.T51 t51
                    	ON town=t51.C0
					Join Db70.dbo.T5 t5
						ON enshab=t5.C0
					Join Db70.dbo.T41 t41
						ON enshab=t41.C0
					Join Db70.dbo.T15 t15
						ON enshab=t15.C0
                    join [Db70].dbo.T7 t7
                    	On noe_va=t7.C0
                    Where id=@id";
        }
        private string GetAllRequestByCustomerNumber(string dbName)
        {
            return $@"Select 
                    	m.radif CustomerNumber,
                    	m.town ZoneId,
                    	t51.C2 ZoneTitle,
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	m.Cod_enshab UsageId,
                    	t41.C1 UsageTitle,
                    	m.TrackingNumber TrackNumber,
                    	m.par_no StringTrackNumber,
                    	m.date_ask RequestDateJalali,
                    	m.date_sabt RegisterDateJalali,
                    	m.s0,
                    	m.s1,
                    	m.s2,
                    	m.s3,
                    	m.s4,
                    	m.s5,
                    	m.s8,
                    	m.s9,
                    	m.s10,
                    	m.s11,
                    	m.s12,
                    	m.s13,
                    	m.s14,
                    	m.s15,
                    	m.s16,
                    	m.s17,
                    	m.s18,
                    	m.s19,
                    	m.s20,
                    	m.s21,
                    	m.s22,
                    	m.s23,
                    	m.s24,
                    	m.s25,
                    	m.s26,
                    	m.s27,
                    	m.s28,
                    	m.s29,
                    	m.s30,
                    	m.s31,
                    	m.s32,
                    	m.s33,
                    	m.s34,
                    	m.s35,
                    	m.s36,
                    	m.s37,
                    	m.s38,
                    	m.s39,
                    	m.s40,
                    	m.s41,
                    	m.s42,
                    	m.s43,
                    	m.s44,
                    	m.s45,
                    	m.s46,
                    	m.s47,
                    	m.s48
                    From [{dbName}].dbo.moshtrak m
                    Join [Db70].dbo.T51 t51 
                    	ON m.town=t51.C0
                    Join [Db70].dbo.T46 t46 
                    	ON t51.C1=t46.C0
                    Join [Db70].dbo.T41 t41 
                    	ON m.cod_enshab=t41.C0
                    Where m.radif=@customerNumber
                    Order By m.date_ask";
        }
    }
}
