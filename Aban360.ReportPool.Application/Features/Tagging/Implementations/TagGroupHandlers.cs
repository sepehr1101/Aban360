using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Implementations
{
    public sealed class CreateTagGroupHandler : ICreateTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public CreateTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        public async Task<int> Handle(CreateTagGroupDto dto)
        {
            return await _service.Create(dto);
        }
    }

    public sealed class GetTagGroupHandler : IGetTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public GetTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        public async Task<TagGroupDto?> Handle(int id)
        {
            return await _service.GetById(id);
        }

        public async Task<IEnumerable<TagGroupDto>> HandleAll()
        {
            return await _service.GetAll();
        }
    }

    public sealed class UpdateTagGroupHandler : IUpdateTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public UpdateTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(UpdateTagGroupDto dto)
        {
            return await _service.Update(dto);
        }
    }

    public sealed class DeleteTagGroupHandler : IDeleteTagGroupHandler
    {
        private readonly ITagGroupService _service;

        public DeleteTagGroupHandler(ITagGroupService service)
        {
            _service = service;
        }

        // Soft delete
        public async Task<bool> Handle(int id)
        {
            return await _service.Delete(id);
        }
    }

    public sealed class TagGroupReportsHandler: ITagGroupReportsHandler
    {
        private readonly ITagGroupReportQueryService _service;
        public TagGroupReportsHandler(ITagGroupReportQueryService service)
        {
            _service = service;
            _service.NotNull(nameof(service));  
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Handle(TagsInputDto inputDto, CancellationToken cancellationToken)
        {
            return await _service.Get(inputDto);
        }
        
        public async Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Handle(TagsInputDto inputDto,bool isZoneTitle, CancellationToken cancellationToken)
        {
            return await _service.Get(inputDto, isZoneTitle);
        }
    }
}