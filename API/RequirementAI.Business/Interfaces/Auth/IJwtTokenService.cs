using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces;

public interface IJwtTokenService
{
    public string GenerateJwt(User user);
    public string GenerateRefreshToken();
}