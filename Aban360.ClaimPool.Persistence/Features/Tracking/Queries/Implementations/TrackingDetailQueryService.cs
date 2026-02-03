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
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<RequestIsRegisterdDto>(query, new { inputDto.TrackNumber });
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
						m.C99 MessageNumber,
						TRIM(m.phone_no) PhoneNumber,
						TRIM(m.Address) Address,
						t.Caller,
						 m.s0, m.s1, m.s2, m.s3, m.s4, m.s5, m.s8, m.s9,
					    m.s10, m.s11, m.s12, m.s13, m.s14, m.s15, m.s16, m.s17, m.s18, m.s19,
					    m.s20, m.s21, m.s22, m.s23, m.s24, m.s25, m.s26, m.s27, m.s28, m.s29,
					    m.s30, m.s31, m.s32, m.s33, m.s34, m.s35, m.s36, m.s37, m.s38, m.s39,
					    m.s40, m.s41, m.s42, m.s43, m.s44, m.s45, m.s46, m.s47, m.s48
					From [{dbName}].dbo.Moshtrak m
					left Join (
						Select 
							TrackNumber,
							BillID,
							NeighbourBillId,
							Caller,
							Rn=Row_Number() Over(Partition By TrackNumber Order By DateAndTime Desc)
						From  [AbAndFazelab].dbo.Tracking 
						)t ON m.TrackingNumber=t.TrackNumber AND t.Rn=1
					Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Join [Db70].dbo.T46 t46 
						ON t51.C1=t46.C0
					where m.trackingNumber=@trackNumber";
        }
    }
}
