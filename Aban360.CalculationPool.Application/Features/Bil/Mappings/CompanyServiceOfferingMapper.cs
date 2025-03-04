﻿using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class CompanyServiceOfferingMapper : Profile
    {
        public CompanyServiceOfferingMapper()
        {
            CreateMap<CompanyServiceOfferingCreateDto, CompanyServiceOffering>().ReverseMap();
            CreateMap<CompanyServiceOfferingDeleteDto, CompanyServiceOffering>().ReverseMap();
            CreateMap<CompanyServiceOfferingUpdateDto, CompanyServiceOffering>().ReverseMap();
            CreateMap<CompanyServiceOfferingGetDto, CompanyServiceOffering>().ReverseMap();
        }
    }
}
