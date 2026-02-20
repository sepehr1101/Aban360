using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class AssessmentTaskQueryService : AbstractBaseConnection, IAssessmentTaskQueryService
    {
        public AssessmentTaskQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<GuidDictionary>> Get(int assessmentCode)
        {
            string query = GetTrackIdsQuery();
            IEnumerable<GuidDictionary> trackIds = await _sqlReportConnection.QueryAsync<GuidDictionary>(query, new { assessmentCode });
            if (!trackIds.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundTask);
            }
            return trackIds;
        }
        public async Task<IEnumerable<AssessmentLocationInfoWithSOutputDto>> GetLocationsInfo(IEnumerable<Guid> trackIds, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            string query = GetTrackDetailQuery(dbName);
            IEnumerable<AssessmentLocationInfoWithSOutputDto> locationsInfo = await _sqlReportConnection.QueryAsync<AssessmentLocationInfoWithSOutputDto>(query, new { trackIds });

            return locationsInfo;
        }

        private string GetTrackIdsQuery()
        {
            return $@"Select 
                        t.ZoneID Id,
                        t.TrackID Title
                    From AbAndFazelab.dbo.Tracking t
                    Join AbAndFazelab.dbo.Examination e
                    	ON t.TrackID=e.TrackId
                    Where 
                    	e.ExaminerCode=@assessmentCode AND
                    	t.Status IN (10,15) AND
                    	e.ResultId IS NULL AND
						t.IsConsiderd=0 AND
	                    DATEADD(MONTH,-3,GETDATE())<=t.DateAndTime";
        }
        private string GetTrackDetailQuery(string dbName)
        {
            return $@"Select
                        t.TrackID,
                    	TRIM(m.mobile) MobileNumber,
                    	TRIM(m.phone_no) PhoneNumber,
                    	m.C99 NotificationMobileNumber,
                    	t.BillID, 
                    	t.NeighbourBillId ,
                    	m.par_no StringTrackNumber,
                        t.TrackNumber TrackNumber,
                    	m.radif CustomerNumber,
                    	t.ServiceGroup_FK ServiceGroupId,
                    	t10.C1 ServiceGroupTitle,
                    	TRIM(m.name) FirstName,
                    	TRIM(m.family) Surname,
                    	TRIM(m.father_nam) FatherName,
                    	m.town ZoneId,
                    	T51.C2 ZoneTitle,
                    	m.ted_takh DiscountCount,
                    	TRIM(m.meli_cod) NationalCode,
                    	m.zarib_f HouseValue,
                    	t.Description ,
                    	m.mojavz IsNonPermanent,
                    	IIF(m.radif>0,1,0) HasCustomerNumber,
                    	(TRIM(m.name)+ ' ' + TRIM(m.family)) FullName,
                    	0 IsVisited,
                    	t15.C1 DiscountTitle,
						m.cod_takh DiscountId,
                    	m.cod_enshab UsageId,
                    	T41.C1 UsageTitle,
                    	m.enshab MeterDiameterId,
                    	t5.C2 MeterDiameterTitle,
                    	e.ExaminerCode AssessmentCode,
                    	e.ExaminerName AssessmentName, 
                    	e.ExaminerMobile AssessmentMobileNumber,
                    	e.DayJalali AssessmentDateJalali,
                        m.s0, m.s1, m.s2, m.s3, m.s4, m.s5, m.s8, m.s9,
					    m.s10, m.s11, m.s12, m.s13, m.s14, m.s15, m.s16, m.s17, m.s18, m.s19,
					    m.s20, m.s21, m.s22, m.s23, m.s24, m.s25, m.s26, m.s27, m.s28, m.s29,
					    m.s30, m.s31, m.s32, m.s33, m.s34, m.s35, m.s36, m.s37, m.s38, m.s39,
					    m.s40, m.s41, m.s42, m.s43, m.s44, m.s45, m.s46, m.s47, m.s48,
						m.sh_no CertificateNumber,
						TRIM(m.address) Address , 
						TRIM(m.POST_COD) PostalCode,
						TRIM(m.eshtrak) ReadingNumber,
						t7.C1 BranchTypeTitle,
						m.noe_va BranchTypeId,
						m.sif_1 Siphon100,
						m.sif_2 Siphon125,
						m.sif_3 Siphon150,
						m.sif_4 Sipohon200,
						m.master_sif MainSiphon,
						m.fix_mas ContractualCapacity,
						TRIM(m.sodor) LicenseIssuanceDateJalali,
						m.arse Premises,
						m.aian ImprovementOverall,
						m.aian_mas ImprovementDomestic,
						m.aian_tej ImprovementCommercial,
						m.tedad_mas DomesticUnit,
						m.tedad_tej CommercialUnit,
						m.tedad_vahd OtherUnit,
						TRIM(m.BLOCK_COD) BlockCode,
						e.FaseleKhakiA TrenchLenW,
						e.FaseleKhakiF TrenchLenS,
						e.FaseleAsphaultA AsphaltLenW,
						e.FaseleAsphaultF AsphaltLenS,
						e.FaseleSangA RockyLenW,
						e.FaseleSangF RockyLenS,
						e.FaseleOtherA OtherLenW,
						e.FaseleOtherF OtherLenS,
						e.OmgheZirzamin BasementDepth
                    From [AbAndFazelab].dbo.Tracking t
                    Join [{dbName}].dbo.moshtrak m
                    	ON t.TrackNumber=m.TrackingNumber
                    Join AbAndFazelab.dbo.Examination e
                    	ON t.TrackID=e.TrackId
                    Join [Db70].dbo.T10	t10 
                    	ON t.ServiceGroup_FK=t10.C0
                    Join [Db70].dbo.t51
                    	ON m.town=T51.C0
                    Join [Db70].dbo.T41
                    	ON m.cod_enshab=T41.C0
                    Join [Db70].dbo.t5
                    	ON m.enshab=T5.C0	
					Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0
					Join Db70.dbo.T15 t15
						ON m.cod_takh=t15.C0
                    Where t.TrackID IN @trackIds";
        }
    }
}
