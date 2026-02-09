using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Implementations
{
    internal sealed class TrackingDetailQueryService : AbstractBaseConnection, ITrackingDetailQueryService
    {
        public TrackingDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<RequestIsRegisterdDto> GetRequestIsRegistered(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetRequestIsRegisteredQuery(dbName);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<RequestIsRegisterdDto>(query, new { inputDto.TrackId });
        }
        public async Task<ExamineTimeSetOutputDto> GetExamineTimeSetDto(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetExaminerResultQuery(dbName);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<ExamineTimeSetOutputDto>(query, new { inputDto.TrackId });
        }
        public async Task<SetExaminationResultOutputDto> GetSetExaminationResultDto(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetExaminerResultQuery(dbName);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<SetExaminationResultOutputDto>(query, new { inputDto.TrackId });
        }
        public async Task<TrackNumberAndDescriptionOutputDto> GetTrackNumberAndDescription(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetTrackNumberAndDescriptionQuery();
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TrackNumberAndDescriptionOutputDto>(query, new { inputDto.TrackId });
        }
        public async Task<CalculationConfirmedDto> GetCalculationConfirmed(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetCalculationConfirmedQuery(dbName);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<CalculationConfirmedDto>(query, new { inputDto.TrackId });
        }
        public async Task<CustomerNumberSpecifiedOutputDto> GetCustomerNumberSpecified(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetCustomerNumberSpecifiedQuery(dbName);
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerNumberSpecifiedOutputDto>(query, new { inputDto.TrackId });
        }
        public async Task<AmountConfirmedOutputDto> GetAmountConfirmed(TrackingDetailGetDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string isRegisterQuery = GetIsRegisterAmountQuery(dbName);
            bool isRegister = await _sqlReportConnection.QueryFirstOrDefaultAsync<bool>(isRegisterQuery, new { inputDto.TrackNumber });

            string tableName = isRegister ? "karten75" : "kart";
            string offeringQuery = GetRegisterOfferingQuery(dbName, tableName);
            IEnumerable<OfferingAmountOutputDto> offerings = await _sqlReportConnection.QueryAsync<OfferingAmountOutputDto>(offeringQuery, new { inputDto.TrackNumber });

            string installmentQuery = GetIstallmentAndpaymentQuery(dbName);
            IEnumerable<InstallmentAndPaymentOutputDto> installments = await _sqlReportConnection.QueryAsync<InstallmentAndPaymentOutputDto>(installmentQuery, new { inputDto.TrackNumber });

            long sumAmount = offerings?.Sum(x => x.Amount) ?? 0;
            long sumDiscount = offerings?.Sum(x => x.Discount) ?? 0;
            return new AmountConfirmedOutputDto()
            {
                Offerings = offerings,
                OfferingAmount = sumAmount,
                OfferingDiscount = sumDiscount,
                OfferingPayable = sumAmount - sumDiscount,


                IstallmentsAndPayments = installments,
                IstallmentAndPaymentAmount = installments?.Sum(x => x.Amount) ?? 0,
            };
        }

        private string GetRequestIsRegisteredQuery(string dbName)
        {
            return @$"Select 
						m.TrackingNumber TrackNumber,
						t.BillID BillId,
						t.NeighbourBillId,
						t51.C2 ZoneTitle,
						m.town ZoneId,
						t46.C2 RegionTitle,
						t46.C1 RegionId,
						TRIM(m.Name) FirstName,
						TRIM(m.family) Surname,
						TRIM(m.father_nam) FatherName,
						TRIM(m.meli_cod) NationalCode,
						TRIM(m.mobile) MobileNumber,
						m.C99 NotificationMobile,
						TRIM(m.phone_no) PhoneNumber,
						TRIM(m.Address) Address,
						t.Caller,
						 m.s0, m.s1, m.s2, m.s3, m.s4, m.s5, m.s8, m.s9,
					    m.s10, m.s11, m.s12, m.s13, m.s14, m.s15, m.s16, m.s17, m.s18, m.s19,
					    m.s20, m.s21, m.s22, m.s23, m.s24, m.s25, m.s26, m.s27, m.s28, m.s29,
					    m.s30, m.s31, m.s32, m.s33, m.s34, m.s35, m.s36, m.s37, m.s38, m.s39,
					    m.s40, m.s41, m.s42, m.s43, m.s44, m.s45, m.s46, m.s47, m.s48
					From  [AbAndFazelab].dbo.Tracking t
					Join [{dbName}].dbo.Moshtrak m
						On t.TrackNumber=m.TrackingNumber
					Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Join [Db70].dbo.T46 t46 
						ON t51.C1=t46.C0
					where t.TrackId=@trackId";
        }
        private string GetExaminerResultQuery(string dbName)
        {
            return $@"Select 
						e.ExaminerCode AssessmentCode,
						e.ExaminerName AssessmentName,
						e.ExaminerMobile AssessmentMobile,
						e.DayJalali AssessmentDayJalali,
						(TRIM(m.name) +' '+ TRIM(m.family)) FullName,
						m.TrackingNumber TrackNumber,
						TRIM(m.Address) Address,
						TRIM(m.mobile) MobileNumber,
						t64.C1 AssessmentResultTitle
					From AbAndFazelab.dbo.Examination e
					Join [{dbName}].dbo.Moshtrak m
						On e.TrackNumber=m.TrackingNumber
					Join [Db70].dbo.T64 t64
						On e.ResultId=t64.C0
					where e.TrackId=@trackId";
        }
        private string GetTrackNumberAndDescriptionQuery()
        {
            return @"Select 
						TrackNumber,
						Description
					From AbAndFazelab.dbo.Tracking
					Where TrackID=@trackId";
        }
        private string GetCalculationConfirmedQuery(string dbName)
        {
            return $@"Select 
						t46.C2 RegionTitle,
						t46.C1 RegionId,
						t51.C2 ZoneTitle,
						m.town ZoneId,
						m.cod_enshab UsageId,
						t41.C1 UsageTitle,
						m.sif_1 Siphon100,
						m.sif_2 Siphon125,
						m.sif_3 Siphon150,
						m.sif_4 Siphon200,
						m.arse Premises,
						m.aian ImprovementOverall,
						m.aian_mas ImprovementDomestic,
						m.aian_tej ImprovementCommertial,
						m.tedad_tej CommertialUnit,
						m.tedad_mas DomesticUnit,
						m.N_Ted_Vhd OtherUnit,
						m.ted_afrad FamilyCount,
						m.ted_khane HouseholdNumber,
						m.cod_takh DiscountTypeId,
						t15.C1 DiscountTypeTitle,
						m.Kargozari EquipmentTitle,
						m.fix_mas ContractualCapacity,
						m.noe_va BranchTypeId,
						t7.C1 BranchTypeTitle,
						m.TrackingNumber TrackNumber,
						t.BillID BillId,
						t.NeighbourBillId,
						m.zarib_f RegionMultiplier,--
						m.CounterType MeterTypeId,
						cv.Title MeterTypeTitle,
						m.enshab MeterDiamterId,
						t5.C2 MeterDiamterTitle,
						m.ted_takh DiscountCount,
						TRIM(m.post_cod) PostalCode,
						m.sharh Description,
					--adam takhfif ab
					--adam takhfif fazelab
					--shomare shenasname					

						TRIM(m.Name) FirstName,
						TRIM(m.family) Surname,
						TRIM(m.father_nam) FatherName,
						TRIM(m.meli_cod) NationalCode,
						TRIM(m.mobile) MobileNumber,
						m.C99 NotificationMobile,
						TRIM(m.phone_no) PhoneNumber,
						TRIM(m.Address) Address,
						 m.s0, m.s1, m.s2, m.s3, m.s4, m.s5, m.s8, m.s9,
					    m.s10, m.s11, m.s12, m.s13, m.s14, m.s15, m.s16, m.s17, m.s18, m.s19,
					    m.s20, m.s21, m.s22, m.s23, m.s24, m.s25, m.s26, m.s27, m.s28, m.s29,
					    m.s30, m.s31, m.s32, m.s33, m.s34, m.s35, m.s36, m.s37, m.s38, m.s39,
					    m.s40, m.s41, m.s42, m.s43, m.s44, m.s45, m.s46, m.s47, m.s48
					From  [AbAndFazelab].dbo.Tracking t
					Join [{dbName}].dbo.Moshtrak m
						On t.TrackNumber=m.TrackingNumber
					Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Join [Db70].dbo.T46 t46 
						ON t51.C1=t46.C0
					Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0
					Join [Db70].dbo.CounterVaziat cv
						ON m.CounterType=cv.MoshtarakinId
					Join [Db70].dbo.T5 t5
						ON m.enshab=t5.C0
					Join [Db70].dbo.T15 t15 
						ON m.cod_takh=t15.C0
					where t.TrackId=@trackId";
        }
        private string GetCustomerNumberSpecifiedQuery(string dbName)
        {
            return @$"Select 
						t.BillID ,
						m.radif CustomerNumber
					From AbAndFazelab.dbo.Tracking t
					Join [{dbName}].dbo.Moshtrak m
						ON t.TrackNumber=m.TrackingNumber
					Where t.TrackID=@trackId";
        }
        private string GetIsRegisterAmountQuery(string dbName)
        {
            return $@"Select 1
					From [{dbName}].dbo.moshtrak
					Where
						TrackingNumber=@trackNumber AND
						sabt = 1 AND
						LEN(TRIM(date_sabt))=10";
        }
        private string GetRegisterOfferingQuery(string dbName, string tableName)
        {
            return $@"Select 
						t100.C1 Title,
						pard Amount,
						takhfif Discount
					From [{dbName}].dbo.{tableName} k
					Join [Db70].dbo.T100 t100
						ON k.noe_bed=t100.C0
					Where LTRIM(k.par_no,'0')=@trackNumber
					Order By t100.C0";
        }
        private string GetIstallmentAndpaymentQuery(string dbName)
        {
            return $@"Select 
						pard Amount,
						mohlat DueDateJalali,
						TRIM(sh_pard1) PayId
					From [{dbName}].dbo.ghest
					where  LTRIM(par_no,'0')=@trackNumber
					Order by mohlat ";
        }

    }
}
