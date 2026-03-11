using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto.AuthDtos;
using RequirementAI.Contract.Dto.ResponseWrappers;
using RequirementAI.Contract.Enums;

namespace RequirementAI.API.Controllers;

public class AuthController(
    IExternalAuthService externalAuthService,
    ILocalAuthService localAuthService)
    : RequirementAIControllerBase
{
    [AllowAnonymous]
    [HttpPost("external-login")]
    public async Task<ActionResult<ResponseDto>> ExternalLogin([FromBody] ExternalAuthRequestDto request,
        CancellationToken ct = default)
    {
        if (request.Provider == AuthProvider.Local)
            return BadRequest(
                ResponseDto.Fail(
                    "Local login is not supported in this endpoint. Use /api/login instead."));

        await externalAuthService.AuthenticateExternalAsync(request, ct);

        return Ok(ResponseDto.Success());
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ResponseDto>> Login(
        [FromBody] LocalAuthRequestDto request,
        CancellationToken ct = default)
    {
        await localAuthService.AuthenticateLocalAsync(request, ct);
        return Ok(ResponseDto.Success());
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ResponseDto>> Logout(CancellationToken ct = default)
    {
        await localAuthService.LogoutAsync(UserId, ct);
        return Ok(ResponseDto.Success());
    }

    [AllowAnonymous]
    [HttpGet("refresh")]
    public async Task<ActionResult<ResponseDto>> Refresh(CancellationToken ct)
    {
        var refreshToken = HttpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(ResponseDto.Fail("Refresh token missing from cookie."));

        await localAuthService.RefreshTokens(refreshToken, ct);

        return Ok(ResponseDto.Success());
    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ResponseDto>> Register(
        [FromBody] LocalRegisterRequestDto request,
        CancellationToken ct = default)
    {
        await localAuthService.Register(request, ct);
        return Ok(ResponseDto.Success());
    }
}