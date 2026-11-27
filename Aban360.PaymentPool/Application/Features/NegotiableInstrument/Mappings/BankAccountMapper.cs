using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Mappings
{
    public class BankAccountMapper : Profile
    {
        public BankAccountMapper()
        {
            CreateMap<BankAccountCreateDto, BankAccount>();
            CreateMap<BankAccountDeleteDto, BankAccount>();
            CreateMap<BankAccountUpdateDto, BankAccount>();
            CreateMap< BankAccount, BankAccountGetDto>()
                .ForMember(dest=>dest.AccountTypeTitle,mem=>mem.MapFrom(x=>x.AccountType.Title));
        }
    }
}
