using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class ConCompanyInsertHandler : AbstractBaseConnection, IConCompanyInsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IValidator<ConCompanyInsertInputDto> _validator;
        public ConCompanyInsertHandler(
            IHttpContextAccessor contextAccessor,
            IValidator<ConCompanyInsertInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }
        public async Task Handle(ConCompanyInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            ConCompanyInsertDto conCompanyInsertDto = GetInsertDto(inputDto, appUser);
            string opLogText = string.Format(OpLogLiterals.ConCompanyInsertOpLog, inputDto.CompanyName, inputDto.RepresentativeName);

            await ExecSql(conCompanyInsertDto, appUser, opLogText);
        }
        private async Task ExecSql(ConCompanyInsertDto conCompanyInsertDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ConCompanyCommandService conCompanyCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await conCompanyCommandService.Insert(conCompanyInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(ConCompanyInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private ConCompanyInsertDto GetInsertDto(ConCompanyInsertInputDto input, IAppUser appUser)
        {
            return new ConCompanyInsertDto()
            {
                ZoneId = input.ZoneId,
                CompanyName = input.CompanyName,
                RepresentativeName = input.RepresentativeName,
                CompanyNationalCode = input.CompanyNationalCode,
                RepresentativeNationalCode = input.RepresentativeNationalCode,
                RepresentativeFatherName = input.RepresentativeFatherName,
                CompanyMobileNumber = input.CompanyMobileNumber,
                RepresentativeMobileNumber = input.RepresentativeMobileNumber,
                CompanyAddress = input.CompanyAddress,
                RepresentativeAddress = input.RepresentativeAddress,
                CompanyPostalCode = input.CompanyPostalCode,
                RepresentativePostalCode = input.RepresentativePostalCode,
                RepresentativeBirthDateJalali = input.RepresentativeBirthDateJalali,
                RepresentativeBirthPlace = input.RepresentativeBirthPlace,
                RepresentativeCertificateNumber = input.RepresentativeCertificateNumber,
                AdministratorName = input.AdministratorName,
                AdministratorMobileNumber = input.AdministratorMobileNumber,
                ContractNumber = input.ContractNumber,
                ContractSubject = input.ContractSubject,
                ContractDataJalali = input.ContractDataJalali,
                ContractDuration = input.ContractDuration,
                ConCompanyPersonnel = "[]",
                InsertedBy = appUser.UserId,
            };
        }
    }
}
