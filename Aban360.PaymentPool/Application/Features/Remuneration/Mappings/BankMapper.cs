using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Mappings
{
    public class BankMapper:Profile
    {
        public BankMapper()
        {
            CreateMap<Bank, BankGetDto>();
        }
    }
}
