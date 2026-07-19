using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Implementations
{
    internal sealed class ConCompanyQueryService : AbstractBaseConnection, IConCompanyQueryService
    {
        public ConCompanyQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<ConCompanyGetDto>> Get()
        {
            string query = GetValidQuery();
            IEnumerable<ConCompanyGetDto> result = await _sqlReportConnection.QueryAsync<ConCompanyGetDto>(query);
            return result;
        }
        public async Task<IEnumerable<ConCompanyGetDto>> GetValidByZoneId(int zoneId)
        {
            string query = GetValidByZoneIdQuery();
            IEnumerable<ConCompanyGetDto> result = await _sqlReportConnection.QueryAsync<ConCompanyGetDto>(query, new { zoneId });
            return result;
        }
        public async Task<ConCompanyGetDto> GetValid(int id)
        {
            string query = GetValidByIdQuery();
            ConCompanyGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConCompanyGetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyId);
            }
            return result;
        }
        public async Task<ConCompanyPersonnelGetDto> GetPersonnel(int id)
        {
            string query = GetValidByIdQuery();
            ConCompanyPersonnelGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConCompanyPersonnelGetDto>(query, new { id });
            if (result is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyId);
            }
            return result;
        }
        public async Task<IEnumerable<ConCompanyPersonnelGetDto>> GetPersonnel()
        {
            string query = GetValidQuery();
            IEnumerable<ConCompanyPersonnelGetDto> result = await _sqlReportConnection.QueryAsync<ConCompanyPersonnelGetDto>(query);
            return result;
        }
        public async Task<ConCompanyPersonnelPersonalGetDto> GetPersonnelById(int companyId, Guid personnelId)
        {
            string query = GetPersonnelByIdQuery();
            ConCompanyPersonnelPersonalGetDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ConCompanyPersonnelPersonalGetDto>(query, new { companyId, personnelId });
            if (result is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyId);
            }
            return result;
        }
        public async Task<int> GetPersonnelIndex(int companyId, Guid personnelId)
        {
            string query = GetPersonnelIndexQuery();
            int? index = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { companyId, personnelId });
            if (index is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidConCompanyPersonnelId);
            }
            return index.Value;
        }

        private string GetValidQuery()
        {
            return @$"Select 
                         c.Id, 
                         c.ZoneId,
						 t51.C2 ZoneTitle, 
                         c.CompanyName,
                         c.CompanyNationalCode, 
                         c.CompanyMobileNumber,
                         c.CompanyAddress, 
                         c.CompanyPostalCode,
                         c.RepresentativePostalCode,
                         c.RepresentativeName,
                         c.RepresentativeNationalCode,
                         c.RepresentativeFatherName,
                         c.RepresentativeMobileNumber,
                         c.RepresentativeAddress,
                         c.RepresentativeBirthDateJalali, 
                         c.RepresentativeBirthPlace,
                         c.RepresentativeCertificateNumber,
                         c.ContractNumber, 
                         c.ContractSubject,
                         c.ContractDataJalali, 
                         c.ContractDuration,
                         c.AdministratorName, 
                         c.AdministratorMobileNumber,
                         c.ConCompanyPersonnel,
                         c.InsertedBy,
                         c.InsertedDateTime,
                         c.RemovedBy,
                         c.RemovedDateTime
                    From [Db70].dbo.ConCompany c
					Left Join [Db70].dbo.T51 t51
						ON t51.C0=c.ZoneId
                    Where c.RemovedBy IS NULL";
        }
        private string GetValidByZoneIdQuery()
        {
            return @$"Select 
                         c.Id, 
                         c.ZoneId,
						 t51.C2 ZoneTitle,
                         c.CompanyName,
                         c.CompanyNationalCode, 
                         c.CompanyMobileNumber,
                         c.CompanyAddress, 
                         c.CompanyPostalCode,
                         c.RepresentativePostalCode,
                         c.RepresentativeName,
                         c.RepresentativeNationalCode,
                         c.RepresentativeFatherName,
                         c.RepresentativeMobileNumber,
                         c.RepresentativeAddress,
                         c.RepresentativeBirthDateJalali, 
                         c.RepresentativeBirthPlace,
                         c.RepresentativeCertificateNumber,
                         c.ContractNumber, 
                         c.ContractSubject,
                         c.ContractDataJalali, 
                         c.ContractDuration,
                         c.AdministratorName, 
                         c.AdministratorMobileNumber,
                         c.ConCompanyPersonnel,
                         c.InsertedBy,
                         c.InsertedDateTime,
                         c.RemovedBy,
                         c.RemovedDateTime
                    From [Db70].dbo.ConCompany c
					Left Join [Db70].dbo.T51 t51
						ON t51.C0=c.ZoneId
                    Where 
                        c.ZoneId = @ZoneId AND
                        c.RemovedBy IS NULL";
        }
        private string GetValidByIdQuery()
        {
            return @$"Select 
                         c.Id,
                         c.ZoneId,
						 t51.C2 ZoneId,
                         c.CompanyName,
                         c.CompanyNationalCode, 
                         c.CompanyMobileNumber,
                         c.CompanyAddress, 
                         c.CompanyPostalCode,
                         c.RepresentativePostalCode,
                         c.RepresentativeName,
                         c.RepresentativeNationalCode,
                         c.RepresentativeFatherName,
                         c.RepresentativeMobileNumber,
                         c.RepresentativeAddress,
                         c.RepresentativeBirthDateJalali, 
                         c.RepresentativeBirthPlace,
                         c.RepresentativeCertificateNumber,
                         c.ContractNumber, 
                         c.ContractSubject,
                         c.ContractDataJalali, 
                         c.ContractDuration,
                         c.AdministratorName, 
                         c.AdministratorMobileNumber, 
                         c.ConCompanyPersonnel,
                         c.InsertedBy,
                         c.InsertedDateTime,
                         c.RemovedBy,
                         c.RemovedDateTime
                    From [Db70].dbo.ConCompany c
					Left Join [Db70].dbo.T51 t51
						ON t51.C0=c.ZoneId
                    Where 
                         c.Id=@Id AND
                         c.RemovedBy IS NULL";
        }
        private string GetPersonnelByIdQuery()
        {
            return @$"SELECT 
                        CAST(JSON_VALUE(value, '$.Id') AS uniqueidentifier) AS Id,
                        JSON_VALUE(value, '$.FullName') AS FullName,
                        JSON_VALUE(value, '$.MobileNumber') AS MobileNumber,
                        JSON_VALUE(value, '$.NationalCode') AS NationalCode
                    FROM [Db70].dbo.ConCompany 
                    CROSS APPLY OPENJSON(ConCompanyPersonnel)
                    WHERE Id = @companyId 
                      AND JSON_VALUE(value, '$.Id') = @personnelId ;";
        }
        private string GetPersonnelIndexQuery()
        {
            return $@"Select Cast([key] as int)
                    From [Db70].dbo.ConCompany 
                    Cross Apply openjson(ConCompanyPersonnel)
                    Where Id=@CompanyId and  JSON_VALUE(value,'$.Id')=@personnelId ";
        }
    }
}
