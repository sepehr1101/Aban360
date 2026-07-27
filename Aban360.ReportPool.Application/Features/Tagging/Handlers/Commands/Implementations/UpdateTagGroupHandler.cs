using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Implementations
{
    public sealed class UpdateTagGroupHandler : IUpdateTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public UpdateTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(UpdateTagGroupDto dto)
        {
            TagGroupDto? result = await _service.GetByStringCode(dto.StringCode);
            if (result is not null)
            {
                throw new CustomValidationException(ExceptionLiterals.InvalidDuplicateStringCode);
            }
            return await _service.Update(dto);
        }
    }
}