using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class QualityAnalysisController(IQualityAnalysisJobService service): RequirementAIControllerBase
{
    [HttpGet("{jobId:guid}")]
    public async Task<IActionResult> GetById(Guid jobId, CancellationToken ct)
    {
        var result = await service.GetById(jobId, ct);
        return Ok(ResponseDto<QualityAnalysisJobDto>.Success(result));
    }
}