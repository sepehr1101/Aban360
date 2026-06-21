using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations
{
    public class ConCompanyCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ConCompanyCommandService(
            IDbConnection sqlRonnection,
            IDbTransaction transaction)
        {
            _connection = sqlRonnection;
            _connection.NotNull(nameof(sqlRonnection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(ConCompanyInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertConCompany);
            }
        }
        public async Task Update(ConCompanyUpdateDto inputDto)
        {
            string command = GetUpdateCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateConCompany);
            }
        }
        public async Task Remove(ConCompanyRemoveDto inputDto)
        {
            string command = GetRemoveCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveConCompany);
            }
        }
        public async Task InsertPersonnel(int companyId, string jsonPersonnelData)
        {
            string command = GetInsertPersonnelCommand();
            int recordCount = await _connection.ExecuteAsync(command, new { companyId, jsonPersonnelData }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertConCompanyPersonnel);
            }
        }
        public async Task UpdatePersonnel(int companyId, string jsonPersonnelData, int index)
        {
            string command = GetUpdatePersonnelCommand(index);
            int recordCount = await _connection.ExecuteAsync(command, new { companyId, jsonPersonnelData, index }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidUpdateConCompanyPersonnel);
            }
        }
        public async Task RemovePersonnel(ConCompanyPersonnelRemoveDto inputDto)
        {
            string command = GetRemovePersonnelCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveConCompanyPersonnel);
            }
        }

        private string GetInsertCommand()
        {
            return @"INSERT INTO [Db70].dbo.ConCompany 
                        (
                            CompanyName, CompanyNationalCode, CompanyMobileNumber, CompanyAddress, CompanyPostalCode, 
                            RepresentativeName, RepresentativeNationalCode, RepresentativeFatherName, RepresentativeMobileNumber, RepresentativeAddress, 
                            RepresentativePostalCode, RepresentativeBirthDateJalali, RepresentativeBirthPlace, RepresentativeCertificateNumber,
                            ContractNumber, ContractSubject, ContractDataJalali, ContractDuration,
                            AdministratorName, AdministratorMobileNumber, ConCompanyPersonnel,
                            InsertedBy, InsertedDateTime
                        )
                    VALUES 
                        (
                            @CompanyName, @CompanyNationalCode, @CompanyMobileNumber, @CompanyAddress, @CompanyPostalCode, 
                            @RepresentativeName, @RepresentativeNationalCode, @RepresentativeFatherName, @RepresentativeMobileNumber, @RepresentativeAddress, 
                            @RepresentativePostalCode, @RepresentativeBirthDateJalali, @RepresentativeBirthPlace, @RepresentativeCertificateNumber,
                            @ContractNumber, @ContractSubject, @ContractDataJalali, @ContractDuration,
                            @AdministratorName, @AdministratorMobileNumber, @ConCompanyPersonnel,                            
                            @InsertedBy, @InsertedDateTime
                        )";
        }
        private string GetUpdateCommand()
        {
            return $@"UPDATE [Db70].dbo.ConCompany 
                    SET 
                            CompanyName = @CompanyName, 
                            CompanyNationalCode = @CompanyNationalCode,
                            CompanyMobileNumber = @CompanyMobileNumber, 
                            CompanyAddress = @CompanyAddress, 
                            CompanyPostalCode = @CompanyPostalCode, 
                            RepresentativePostalCode  = @RepresentativePostalCode ,
                            RepresentativeName = @RepresentativeName, 
                            RepresentativeNationalCode = @RepresentativeNationalCode,
                            RepresentativeFatherName = @RepresentativeFatherName, 
                            RepresentativeMobileNumber = @RepresentativeMobileNumber, 
                            RepresentativeAddress = @RepresentativeAddress, 
                            RepresentativeBirthDateJalali = @RepresentativeBirthDateJalali, 
                            RepresentativeBirthPlace = @RepresentativeBirthPlace, 
                            RepresentativeCertificateNumber = @RepresentativeCertificateNumber,
                            ContractNumber = @ContractNumber, 
                            ContractSubject = @ContractSubject, 
                            ContractDataJalali = @ContractDataJalali, 
                            ContractDuration = @ContractDuration,
                            AdministratorName = @AdministratorName, 
                            AdministratorMobileNumber = @AdministratorMobileNumber 
                    WHERE Id = @Id;";
        }
        private string GetRemoveCommand()
        {
            return $@"UPDATE [Db70].dbo.ConCompany 
                    SET 
                        RemovedBy = @RemovedBy , 
                        RemovedDateTime = @RemovedDateTime
                    WHERE Id = @Id; ";
        }
        private string GetInsertPersonnelCommand()
        {
            return $@"UPDATE [Db70].dbo.ConCompany
                    SET ConCompanyPersonnel = JSON_MODIFY(ConCompanyPersonnel, 'append $', JSON_QUERY(@jsonPersonnelData))
                    WHERE Id = @companyId ;";
        }
        private string GetUpdatePersonnelCommand(int index)
        {
            return $@" UPDATE [Db70].dbo.ConCompany
                   SET ConCompanyPersonnel =
                       JSON_MODIFY(
                           ConCompanyPersonnel,
                           '$[{index}]',
                           JSON_QUERY(@jsonPersonnelData)
                       )
                   WHERE Id = @CompanyId;";
        }
        private string GetRemovePersonnelCommand()
        {
            return $@"UPDATE [Db70].dbo.ConCompany
                    SET 
                    	ConCompanyPersonnel = JSON_MODIFY(
                                                    JSON_MODIFY(
                                                        ConCompanyPersonnel,
                                                        CONCAT('$[', @Index, '].RemovedBy'),
                                                       CAST(@RemovedBy AS nvarchar(36))
                                                    ),
                                                    CONCAT('$[', @Index, '].RemovedDateTime'),
                                                    CONVERT(nvarchar(30), @RemovedDateTime, 126)
                                               )
                    WHERE Id = @CompanyId;";
        }
    }
}
