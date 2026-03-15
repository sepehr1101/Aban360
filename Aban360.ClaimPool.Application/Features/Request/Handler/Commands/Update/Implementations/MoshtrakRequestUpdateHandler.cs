using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Update.Implementations
{
    internal sealed class MoshtrakRequestUpdateHandler : AbstractBaseConnection, IMoshtrakRequestUpdateHandler
    {
        private readonly IValidator<MoshtrakUpdateInputDto> _validator;
        public MoshtrakRequestUpdateHandler(
            IValidator<MoshtrakUpdateInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            if (inputDto.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.CantUpdate);
            }
            MoshtrakUpdateInfoDto moshtrakUpdateDto=GetUpdateDto(inputDto);
            string dbName = GetDbName(inputDto.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    await _moshtrackCommandService.Update(moshtrakUpdateDto, dbName);
                    transaction.Commit();
                }
            }
        }
        private async Task InputValidation(MoshtrakUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private MoshtrakUpdateInfoDto GetUpdateDto(MoshtrakUpdateInputDto inputDto)
        {
            MoshtrakServiceDto serviceSelected = MoshtrakService.GetServicesSelected(inputDto.SelectedServices);

            return new MoshtrakUpdateInfoDto()
            {
                TrackNumber = inputDto.TrackNumber,
                CustomerNumber = inputDto.CustomerNumber,
                ZoneId = inputDto.ZoneId,
                NotificationMobile = inputDto.NotificationMobile,
                UsageId = inputDto.UsageId,
                MeterDiameterId = inputDto.MeterDiameterId,
                BranchTypeId = inputDto.BranchTypeId,
                DiscountCount = inputDto.DiscountCount,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                Premises = inputDto.Premises,
                ImprovementCommercial = inputDto.ImprovementCommercial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                ContractualCapacity = inputDto.ContractualCapacity,
                HouseValue = inputDto.HouseValue,
                CommercialUnit = inputDto.CommercialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                DiscountTypeId = inputDto.DiscountTypeId,
                NationalCode = inputDto.NationalCode,
                FatherName = inputDto.FatherName,
                PostalCode = inputDto.PostalCode,
                IsNonPermanent = inputDto.IsNonPermanent,
                Address = inputDto.Address,
                CounterType = inputDto.CounterType,
                MainSiphon = inputDto.MainSiphon,
                CommonSiphon = inputDto.CommonSiphon,
                Description=inputDto.Description,
                IsSpecial=inputDto.IsSpecial,
                ReadingNumber=inputDto.ReadingNumber,
               

                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }
    }
}
