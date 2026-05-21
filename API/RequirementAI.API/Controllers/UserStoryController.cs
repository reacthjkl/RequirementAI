using Microsoft.AspNetCore.Mvc;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Business.Services;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Dto.ResponseWrappers;

namespace RequirementAI.API.Controllers;

public class UserStoryController(IUserStoryService userStoryService) : RequirementAIControllerBase
{
}