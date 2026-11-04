using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Aban360.UserPool.Application.Features.SubscriberRequests;

public sealed class SubscriberRequestOtp
{
    private readonly byte[] _key;

    public SubscriberRequestOtp(SubscriberRequestOptions options)
    {
        if (Encoding.UTF8.GetByteCount(options.OtpHashKey) < 32)
            throw new InvalidOperationException("SubscriberRequests:OtpHashKey must contain at least 32 bytes and be shared by all API instances.");
        _key = Encoding.UTF8.GetBytes(options.OtpHashKey);
    }

    public string Generate() => RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6", CultureInfo.InvariantCulture);
    public byte[] Hash(Guid requestId, Guid challengeId, string code) =>
        HMACSHA256.HashData(_key, Encoding.UTF8.GetBytes($"{requestId:N}:{challengeId:N}:{code}"));

    public bool Verify(Guid requestId, Guid challengeId, string code, byte[] hash) =>
        code.Length == 6 && code.All(c => c is >= '0' and <= '9') &&
        CryptographicOperations.FixedTimeEquals(Hash(requestId, challengeId, code), hash);
}
