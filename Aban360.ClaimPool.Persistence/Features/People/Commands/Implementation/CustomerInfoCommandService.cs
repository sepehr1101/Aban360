using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.People.Commands.Implementation
{
    internal sealed class CustomerInfoCommandService : AbstractBaseConnection, ICustomerInfoCommandService
    {
        public CustomerInfoCommandService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Update(CustomerInfoUpdateDto updateDto)
        {
            string customerInfoUpdateQueryString = GetCustomerInfoUpdateQuery(updateDto.ZoneId);
            var @params = new
            {
                billId=updateDto.BillId,
                readingNumber = updateDto.ReadingNumber,
                firstName = updateDto.FirstName,
                surname = updateDto.Surname,
                fatherName = updateDto.FatherName,
                meterDiameterId = updateDto.MeterDiameterId,
                usageId = updateDto.UsageId,
                otherUnit = updateDto.OtherUnit,
                commercialUnit = updateDto.CommercialUnit,
                householdNumber = updateDto.HouseholdNumber,
                domesticUnit = updateDto.DomesticUnit,
                registerDateJalali = updateDto.RegisterDateJalali,
                primises = updateDto.Premises,
                overollImprovement = updateDto.OverollImprovement,
                commercialImprovement = updateDto.CommercialImprovement,
                domesticImprovement = updateDto.DomesticImprovement,
                meterRequestDateJalali = updateDto.MeterRequestDateJalali,
                meterInstallationDateJalali = updateDto.MeterInstallationDateJalali,
                siphonRequestDateJalali = updateDto.SiphonRequestDateJalali,
                siphonInstallationDateJalali = updateDto.SiphonInstallationDateJalali,
                address = updateDto.Address,
                housePlate = updateDto.HousePlate,
                isSpecial = updateDto.IsSpecial,
                deletionStateId = updateDto.DeletionStateId,
                seStateId = updateDto.UseStateId,
                mainSiphon = updateDto.MainSiphon,
                siphon1 = updateDto.Siphon1,
                siphon2 = updateDto.Siphon2,
                siphon3 = updateDto.Siphon3,
                siphon4 = updateDto.Siphon4,
                siphon5 = updateDto.Siphon5,
                siphon6 = updateDto.Siphon6,
                siphon7 = updateDto.Siphon7,
                siphon8 = updateDto.Siphon8,
                commonSiphon1 = updateDto.CommonSiphon1,
                contractualCapacity = updateDto.ContractualCapacity,
                bodySerial = updateDto.BodySerial,
                waterInstalltionRegistareDate = updateDto.WaterInstalltionRegistareDate,
                sewageInstalltionRegistareDate = updateDto.SewageInstalltionRegistareDate,
                postalCode = updateDto.PostalCode,
                phoneNumber = updateDto.PhoneNumber,
                mobile = updateDto.MobileNumber,
                nationalCode = updateDto.NationalCode,
                mojavz = updateDto.MOJAVZ,
                villageId = updateDto.VillageId,
                villageName = updateDto.VillageName,
                x = updateDto.X,
                y = updateDto.Y,
                emptyUnit = updateDto.EmptyUnit,
                Operator = updateDto.Operator,
                guild = updateDto.Guild,
                date_Khane = updateDto.date_KHANE
            };
            var result = await _sqlReportConnection.ExecuteAsync(customerInfoUpdateQueryString, @params);
        }

        private string GetCustomerInfoUpdateQuery(int zoneId)
        {
            return @$"UPDATE [{zoneId}].dbo.members 
                     SET
                     	eshtrak = ISNULL(@readingNumber, eshtrak),
                     	name    = ISNULL(@firstName, name),
                     	family  = ISNULL(@surname, family),
                     	father_nam  = ISNULL(@fatherName, father_nam),
                     	enshab = ISNULL(@meterDiameterId, enshab),
                     	cod_enshab  = ISNULL(@usageId, cod_enshab),
                     	tedad_vahd  = ISNULL(@otherUnit, tedad_vahd),
                     	tedad_mas = ISNULL(@commercialUnit, tedad_mas),
                     	ted_khane = ISNULL(@householdNumber, ted_khane),
                     	tedad_tej = ISNULL(@domesticUnit, tedad_tej),
                     	date_sabt = ISNULL(@registerDateJalali, date_sabt),
                     	arse     = ISNULL(@primises, arse),
                     	aian     = ISNULL(@overollImprovement, aian),
                     	aian_mas = ISNULL(@commercialImprovement, aian_mas),
                     	aian_tej = ISNULL(@domesticImprovement, aian_tej),
                     	ask_ab   = ISNULL(@meterRequestDateJalali, ask_ab),
                     	inst_ab  = ISNULL(@meterInstallationDateJalali, inst_ab),
                     	ask_fas  = ISNULL(@siphonRequestDateJalali, ask_fas),
                     	inst_fas = ISNULL(@siphonInstallationDateJalali, inst_fas),
                     	address  = ISNULL(@address, address),
                     	pelak    = ISNULL(@housePlate, pelak),
                     	edareh_k = ISNULL(@isSpecial, edareh_k),
                     	hasf     = ISNULL(@deletionStateId, hasf),
                     	noe_va   = ISNULL(@seStateId, noe_va),
                     	master_sif  = ISNULL(@mainSiphon, master_sif),
                     	sif_1 = ISNULL(@siphon1, sif_1),
                     	sif_2 = ISNULL(@siphon2, sif_2),
                     	sif_3 = ISNULL(@siphon3, sif_3),
                     	sif_4 = ISNULL(@siphon4, sif_4),
                     	sif_5 = ISNULL(@siphon5, sif_5),
                     	sif_6 = ISNULL(@siphon6, sif_6),
                     	sif_7 = ISNULL(@siphon7, sif_7),
                     	sif_8 = ISNULL(@siphon8, sif_8),
                     	sif_mosh_1  = ISNULL(@commonSiphon1, sif_mosh_1),
                     	fix_mas = ISNULL(@contractualCapacity, fix_mas),
                     	serial_co= ISNULL(@bodySerial, serial_co),
                     	G_inst_ab= ISNULL(@waterInstalltionRegistareDate, G_inst_ab),
                     	G_inst_fas  = ISNULL(@sewageInstalltionRegistareDate, G_inst_fas),
                     	POST_COD = ISNULL(@postalCode, POST_COD),
                     	PHONE_NO = ISNULL(@phoneNumber, PHONE_NO),
                     	MOBILE   = ISNULL(@mobile, MOBILE),
                     	MELI_COD = ISNULL(@nationalCode, MELI_COD),
                     	MOJAVZ   = ISNULL(@mojavz, MOJAVZ),
                     	VillageId= ISNULL(@villageId, VillageId),
                     	VillageName = ISNULL(@villageName, VillageName),
                     	X = ISNULL(@x, X),
                     	Y = ISNULL(@y, Y),
                     	Khali_s = ISNULL(@emptyUnit, Khali_s),
                     	operator = ISNULL(@operator, operator),
                     	Senf  = ISNULL(@guild, Senf),
                     	date_KHANE  = ISNULL(@date_Khane, date_KHANE)
                     WHERE bill_id = @billId";
        }
    }
}
