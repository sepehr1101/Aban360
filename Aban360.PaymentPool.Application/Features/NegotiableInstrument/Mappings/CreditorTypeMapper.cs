﻿using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Entities;
using AutoMapper;

namespace Aban360.PaymentPool.Application.Features.NegotiableInstrument.Mappings
{
    public class CreditorTypeMapper : Profile
    {
        public CreditorTypeMapper()
        {
            CreateMap< CreditorType, CreditorTypeGetDto>();
        }
    }
}
