using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.Remuneration.Entities;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.Remuneration.Mappings
{
    public class PaymentProcedureMapper:Profile
    {
        public PaymentProcedureMapper()
        {
            CreateMap<PaymentProcedure, PaymentProcedureGetDto>();
        }
    }
}
