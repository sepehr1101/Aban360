using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class IndividualsInfoService : AbstractBaseConnection, IIndividualsInfoService
    {
        public IndividualsInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<IndividualsInfoDto>> GetInfo(string billId)
        {
            string individualsQuery = GetIndividualsSummayDtoQuery();
            IEnumerable<IndividualsInfoDto> result = await _sqlConnection.QueryAsync<IndividualsInfoDto>(individualsQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"select 
                	i.FullName,
                	i.FatherName,
                	i.NationalId,
                	i.PhoneNumbers,
                	i.MobileNumbers,
                    e.HouseholdNumber,
                	ier.Title AS IndividualEstateRelationType,
                	COALESCE(d.Title, '-') AS DiscountType
                from ClaimPool.WaterMeter w 
                join ClaimPool.Estate e on w.EstateId=e.Id
                join ClaimPool.IndividualEstate ie on ie.EstateId=e.Id
                join ClaimPool.Individual i on i.id=ie.IndividualId
                join ClaimPool.IndividualEstateRelationType ier on ie.IndividualEstateRelationTypeId=ier.Id
                LEFT join ClaimPool.IndividualDiscountType idt on i.Id=idt.IndividualId
                LEFT join ClaimPool.DiscountType d on idt.DiscountTypeId=d.Id
                where w.BillId=@billId";
        }
    }
}
