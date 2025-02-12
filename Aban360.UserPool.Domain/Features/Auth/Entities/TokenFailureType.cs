using Aban360.UserPool.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Auth.Entities
{
    [Table(nameof(TokenFailureType))]
    public class TokenFailureType
    {
        public TokenFailureTypeEnum Id { get; set; }
        public string Title { get; set; } = default!;
    }
}
