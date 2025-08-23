using Aban260.BlobPool.Infrastructure.Features.DmsServices.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts
{
    public interface IUserGetHandler
    {
        Task<string> Handle(SearchUserInputDto input);
    }
    internal sealed class UserGetHandler : IUserGetHandler
    {
        private readonly ISearchUserServices _finedUserSevice;
        public UserGetHandler(ISearchUserServices finedUserSevice)
        {
            _finedUserSevice = finedUserSevice;
            _finedUserSevice.NotNull(nameof(finedUserSevice));
        }

        public async Task<string> Handle(SearchUserInputDto input)
        {
            string result = await _finedUserSevice.Services(input.FolderPath,input.Property,input.Path);
            return result;
        }
    }   
}
