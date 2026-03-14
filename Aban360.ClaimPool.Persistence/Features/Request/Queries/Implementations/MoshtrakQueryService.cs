using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
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
        public async Task<IEnumerable<MoshtrakOutputDto>> Get(MoshtrakGetDto inputDto, MoshtrakSearchTypeEnum searchType)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetInfoByConditionQuery(dbName, GetCondition(searchType));
            IEnumerable<MoshtrakOutputDto> result = await _sqlReportConnection.QueryAsync<MoshtrakOutputDto>(query, inputDto);
            if (!result.Any())
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
                    	address Address,
                    	post_cod PostalCode,
                    	NeighbourBillID,
                    	TrackingNumber TrackNumber,
                        cod_enshab UsageId,
                        Sabt IsRegistered
                    From [{dbName}].dbo.moshtrak 
                    Join Db70.dbo.T51 t51
                    	ON town=t51.C0
                    where {condition}
                    Order By date_ask Desc";
        }
    }
}
