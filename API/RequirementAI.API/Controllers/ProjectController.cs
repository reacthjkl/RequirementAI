using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class ProjectController(IProjectService projectService): RequirementAIControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjects(CancellationToken ct)
    {
        var projects = await projectService.GetByOrganizationId(OrganizationId, ct);
        return Ok(ResponseDto<List<ProjectResponseDto>>.Success(projects));
    }
}