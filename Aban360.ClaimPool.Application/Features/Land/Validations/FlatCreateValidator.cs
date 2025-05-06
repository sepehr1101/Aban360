using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class FlatCreateValidator:BaseValidator<FlatCreateDto>
    {
        public FlatCreateValidator()
        {
            RuleFor(f => f.EstateId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);
            
            RuleFor(f => f.PostalCode)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);
        }
    }
    public class CapacityCalculationIndexCreateValidator : BaseValidator<CapacityCalculationIndexCreateDto>
    {
        public CapacityCalculationIndexCreateValidator()
        {
           
        }
    }
    public class CapacityCalculationIndexUpdateValidator : BaseValidator<CapacityCalculationIndexUpdateDto>
    {
        public CapacityCalculationIndexUpdateValidator()
        {
            
        }
    }
    public class ConstructionTypeUpdateValidator : BaseValidator<ConstructionTypeUpdateDto>
    {
        public ConstructionTypeUpdateValidator()
        {
            
        }
    }
    public class ConstructionTypeCreateValidator : BaseValidator<ConstructionTypeCreateDto>
    {
        public ConstructionTypeCreateValidator()
        {
            
        }
    }
    public class EstateBoundTypeCreateValidator : BaseValidator<EstateBoundTypeCreateDto>
    {
        public EstateBoundTypeCreateValidator()
        {
            
        }
    }
    public class EstateBoundTypeUpdateValidator : BaseValidator<EstateBoundTypeUpdateDto>
    {
        public EstateBoundTypeUpdateValidator()
        {
            
        }
    }
    public class EstateWaterResourceUpdateValidator : BaseValidator<EstateWaterResourceUpdateDto>
    {
        public EstateWaterResourceUpdateValidator()
        {
            
        }
    }
    public class EstateWaterResourceCreateValidator : BaseValidator<EstateWaterResourceCreateDto>
    {
        public EstateWaterResourceCreateValidator()
        {
            
        }
    }
    public class GuildCreateValidator : BaseValidator<GuildCreateDto>
    {
        public GuildCreateValidator()
        {
            
        }
    }
    public class GuildUpdateValidator : BaseValidator<GuildUpdateDto>
    {
        public GuildUpdateValidator()
        {
            
        }
    }
    public class HandoverUpdateValidator : BaseValidator<HandoverUpdateDto>
    {
        public HandoverUpdateValidator()
        {
            
        }
    }
    public class HandoverCreateValidator : BaseValidator<HandoverCreateDto>
    {
        public HandoverCreateValidator()
        {
            
        }
    }
    public class OfficialHolidayCreateValidator : BaseValidator<OfficialHolidayCreateDto>
    {
        public OfficialHolidayCreateValidator()
        {
            
        }
    }
    public class OfficialHolidayUpdateValidator : BaseValidator<OfficialHolidayUpdateDto>
    {
        public OfficialHolidayUpdateValidator()
        {
            
        }
    }
    public class ProfessionUpdateValidator : BaseValidator<ProfessionUpdateDto>
    {
        public ProfessionUpdateValidator()
        {
            
        }
    }
    public class ProfessionCreateValidator : BaseValidator<ProfessionCreateDto>
    {
        public ProfessionCreateValidator()
        {
            
        }
    }
    public class UsageLevel1CreateValidator : BaseValidator<UsageLevel1CreateDto>
    {
        public UsageLevel1CreateValidator()
        {
            
        }
    }
    public class UsageLevel2CreateValidator : BaseValidator<UsageLevel2CreateDto>
    {
        public UsageLevel2CreateValidator()
        {
            
        }
    }
    public class UsageLevel3CreateValidator : BaseValidator<UsageLevel3CreateDto>
    {
        public UsageLevel3CreateValidator()
        {
            
        }
    }
    public class UsageLevel4CreateValidator : BaseValidator<UsageLevel4CreateDto>
    {
        public UsageLevel4CreateValidator()
        {
            
        }
    }
    public class UsageLevel4UpdateValidator : BaseValidator<UsageLevel4UpdateDto>
    {
        public UsageLevel4UpdateValidator()
        {
            
        }
    }
    public class UsageLevel3UpdateValidator : BaseValidator<UsageLevel3UpdateDto>
    {
        public UsageLevel3UpdateValidator()
        {
            
        }
    }
    public class UsageLevel2UpdateValidator : BaseValidator<UsageLevel2UpdateDto>
    {
        public UsageLevel2UpdateValidator()
        {
            
        }
    }
    public class UsageLevel1UpdateValidator : BaseValidator<UsageLevel1UpdateDto>
    {
        public UsageLevel1UpdateValidator()
        {
            
        }
    }
    public class UsageUpdateValidator : BaseValidator<UsageUpdateDto>
    {
        public UsageUpdateValidator()
        {
            
        }
    }
    public class UsageCreateValidator : BaseValidator<UsageCreateDto>
    {
        public UsageCreateValidator()
        {
            
        }
    }
    public class UserLeaveCreateValidator : BaseValidator<UserLeaveCreateDto>
    {
        public UserLeaveCreateValidator()
        {
            
        }
    }
    public class UserLeaveUpdateValidator : BaseValidator<UserLeaveUpdateDto>
    {
        public UserLeaveUpdateValidator()
        {
            
        }
    }
    public class UserWorkdayUpdateValidator : BaseValidator<UserWorkdayUpdateDto>
    {
        public UserWorkdayUpdateValidator()
        {
            
        }
    }
    public class UserWorkdayCreateValidator : BaseValidator<UserWorkdayCreateDto>
    {
        public UserWorkdayCreateValidator()
        {
            
        }
    }
    public class WaterResourceCreateValidator : BaseValidator<WaterResourceCreateDto>
    {
        public WaterResourceCreateValidator()
        {
            
        }
    }
    public class WaterResourceUpdateValidator : BaseValidator<WaterResourceUpdateDto>
    {
        public WaterResourceUpdateValidator()
        {
            
        }
    }
}
