using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;

namespace RequirementAI.Business.Providers;

public class GoogleAuthProvider(IConfiguration config, IMapper mapper) : IExternalAuthProvider
{
    private readonly string _clientId = config["Authentication:Google:ClientId"]
                                        ?? throw new InvalidOperationException(
                                            "Missing Google Client ID configuration.");

    public AuthProvider Provider => AuthProvider.Google;

    public async Task<UserIdentityPayload?> ValidateAsync(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_clientId]
        });

        return mapper.Map<UserIdentityPayload>(payload);
    }
}