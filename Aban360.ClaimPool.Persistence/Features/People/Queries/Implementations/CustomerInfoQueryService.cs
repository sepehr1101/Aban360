using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.People.Queries.Implementations
{
    internal sealed class CustomerInfoQueryService : AbstractBaseConnection, ICustomerInfoQueryService
    {
        public CustomerInfoQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<CustomerInfoGetDto> Get(string billId)
        {
            string customerInfoQueryString = GetCustomerInfoQuery();
            CustomerInfoGetDto customerInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoGetDto>(customerInfoQueryString, new { billId });
            return customerInfo;
        }

        private string GetCustomerInfoQuery()
        {
            return @"Select
						c.BillId,
						c.ZoneId,
						c.ReadingNumber,
						c.FirstName,
						c.SureName AS Surname,
						c.FatherName,
						c.WaterDiameterId,
						c.UsageId,
						c.OtherCount AS OtherUnit,
						c.CommercialCount AS CommercialUnit,
						c.FamilyCount AS HouseholdNumber,
						c.DomesticCount AS DomesticUnit,
						c.RegisterDayJalali AS RegisterDateJalali,
						c.FieldArea AS Premises,
						c.ConstructedArea AS OverallImprovement,
						c.CommercialArea AS CommercialImprovement,
						c.DomesticArea AS DomesticImprovement,
						c.WaterRequestDate AS MeterRequestDateJalali,
						c.WaterInstallDate AS MeterInstallationDateJalali,
						c.SewageRequestDate AS SiphonRequestDateJalali,--
						c.SewageRequestDate AS SiphonInstallationDateJalali,
						c.Address ,
						'' AS HousePlate,
						c.IsSpecial,
						c.DeletionStateId,
						c.BranchType AS UseStateId,
						c.MainSiphonTitle AS MainSiphon,
						c.Siphon100 AS Siphon1,
						c.Siphon125 AS Siphon2,
						c.Siphon150 AS Siphon3,
						c.Siphon200 AS Siphon4,
						c.Siphon5 AS Siphon5,
						c.Siphon6 AS Siphon6,
						c.Siphon7 AS Siphon7,
						c.Siphon8 AS Siphon8,
						c.HasCommonSiphon AS CommonSiphonq1,
						c.ContractCapacity AS ContractualCapacity,
						c.MeterSerialBody AS BodySerial,
						'' AS WaterInstalltionRegistareDate,
						'' AS SewageInstalltionRegistareDate,
						c.PostalCode,
						c.PhoneNo AS PhoneNumber,
						c.MobileNo AS MobileNumber,
						c.NationalId AS NationalCode,
						0 AS MOJAVZ,
						c.VillageId,
						c.VillageName,
						x AS X,
						y AS Y,
						EmptyCount AS EmptyUnit,
						0 AS Operator,
						c.GuildId AS Guild,
						c.HouseholdDateJalali 
					From [CustomerWarehouse].dbo.Clients c
					Where
						c.BillId=@billId AND
						c.ToDayJalali IS NULL";
        }
    }
}
