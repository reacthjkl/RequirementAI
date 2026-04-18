using AutoMapper;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Enums;
using RequirementAI.Contract.Exceptions;

namespace RequirementAI.Business.Providers.Auth;

public class GoogleAuthProvider(IConfiguration config, IMapper mapper) : IExternalAuthProvider
{
    private readonly string _clientId = config["Authentication:Google:ClientId"]
                                        ?? throw new AuthorizationException(
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