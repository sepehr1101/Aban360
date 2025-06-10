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
            //string individualsQuery = GetIndividualsSummayDtoQuery();
            string individualsQuery = GetIndividualsSummayDtoWithClientDBQuery();
            IEnumerable<IndividualsInfoDto> result = await _sqlReportConnection.QueryAsync<IndividualsInfoDto>(individualsQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"select 
                	i.FirstName,
                    i.Surname,
                    i.FirstName+N' '+i.Surname as FullName,
                    i.Surname,
                	i.FatherName,
                	i.NationalId,
                	i.PhoneNumbers,
                	i.MobileNumbers,
                	ier.Title AS IndividualEstateRelationType,
                    0 as 'NumberOfPeople',
                    e.HouseholdNumber,
                	COALESCE(d.Title, '-') AS DiscountType,
                    0 as 'IsOwnerAgent'
                from ClaimPool.WaterMeter w 
                join ClaimPool.Estate e on w.EstateId=e.Id
                join ClaimPool.IndividualEstate ie on ie.EstateId=e.Id
                join ClaimPool.Individual i on i.id=ie.IndividualId
                join ClaimPool.IndividualEstateRelationType ier on ie.IndividualEstateRelationTypeId=ier.Id
                LEFT join ClaimPool.IndividualDiscountType idt on i.Id=idt.IndividualId
                LEFT join ClaimPool.DiscountType d on idt.DiscountTypeId=d.Id
                where w.BillId=@billId";
        }

        private string GetIndividualsSummayDtoWithClientDBQuery()
        {
            return @"select 
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.FirstName+' '+c.SureName AS FullName,
                    	c.NationalId,
                    	c.FatherName,
                    	c.PhoneNo AS PhoneNumbers,
                    	c.MobileNo AS MobileNumbers,
                    	N'مالک' AS IndividualEstateRelationType,
                    	c.FamilyCount AS HouseholdNumber,
                    	0 AS NumberOfPeople,
                    	c.DiscountTypeTitle AS DiscountType
                    	1 AS IsOwnerAgent
                    from [CustomerWarehouse].dbo.Clients c
                    where c.BillId=@billId
                    and c.ToDayJalali is null";
        }
    }
}
