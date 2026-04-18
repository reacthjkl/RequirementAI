using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class AuthController(
    IAuthService authService)
    : RequirementAIControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ResponseDto>> Login(
        [FromBody] AuthRequestDto request,
        CancellationToken ct = default)
    {
        await authService.AuthenticateLocalAsync(request, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ResponseDto>> Logout(CancellationToken ct = default)
    {
        await authService.LogoutAsync(UserId, ct);
        return Ok(ResponseDto.Success());
    }

    [AllowAnonymous]
    [HttpGet("refresh")]
    public async Task<ActionResult<ResponseDto>> Refresh(CancellationToken ct)
    {
        var refreshToken = HttpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(ResponseDto.Fail("Refresh token missing from cookie."));

        await authService.RefreshTokens(refreshToken, ct);

        return Ok(ResponseDto.Success());
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ResponseDto>> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken ct = default)
    {
        await authService.Register(request, ct);
        return Ok(ResponseDto.Success());
    }
}