using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Helpers;

public class Argon2PasswordHasher(IOptions<Argon2Settings> settings) : IPasswordHasher
{
    private readonly Argon2Settings _settings = settings.Value;

    public string Hash(string password)
    {
        var config = new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            TimeCost = _settings.TimeCost,
            MemoryCost = _settings.MemoryCost,
            Lanes = _settings.Lanes,
            Threads = Environment.ProcessorCount,
            Password = Encoding.UTF8.GetBytes(password),
            Salt = GenerateSalt()
        };

        using var argon2 = new Argon2(config);
        using var hash = argon2.Hash();
        return config.EncodeString(hash.Buffer);
    }

    public bool Verify(string password, string encodedHash)
    {
        return Argon2.Verify(encodedHash, password);
    }

    private byte[] GenerateSalt()
    {
        var salt = new byte[_settings.SaltLength];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }
}